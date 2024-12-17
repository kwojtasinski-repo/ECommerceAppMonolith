using ECommerce.Modules.Currencies.Core.Scheduler;
using ECommerce.Modules.Currencies.Tests.Integration.Common;
using ECommerce.Shared.Abstractions.SchedulerJobs;
using Flurl.Http;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace ECommerce.Modules.Currencies.Tests.Integration.Schedulers
{
    public class CurrencyRateDownloaderTests : CurrenciesBaseTest
    {
        [Fact]
        public async Task should_add_rates()
        {
            var content = Common.Extensions.GetCurrencyRateTableJsonString();
            var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
            _wireMockServer.Given(
              Request.Create()
                .WithPath($"/api/exchangerates/tables/a")
                .UsingGet()).RespondWith(
              Response.Create()
                .WithBody(content)
                .WithStatusCode(200));

            await _currencyRateDownloader.DoWork(CancellationToken.None);

            var rates = await dbContext.CurrencyRates.Where(cr => cr.CurrencyDate == currentDate).ToListAsync();
            rates.ShouldNotBeNull();
            rates.ShouldNotBeEmpty();
            rates.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task given_proper_code_should_fail_when_server_not_respond_after_2_seconds()
        {
            _wireMockServer.Given(
              Request.Create()
                .WithPath($"/api/exchangerates/tables/a")
                .UsingGet()).RespondWith(
              Response.Create()
                .WithStatusCode(200)
                // Return response after 5 seconds
                .WithDelay(5.Seconds()));

            // Expect exception to be thrown related to HTTP connection
            // Because according to configuration, the HTTP response should be established
            // within 2 seconds
            var exception = await Record.ExceptionAsync(() => _currencyRateDownloader.DoWork(CancellationToken.None));
            exception.ShouldBeOfType<FlurlHttpTimeoutException>();
        }

        private readonly WireMockServer _wireMockServer;
        private readonly ISchedulerTask<CurrencyRateScheduler> _currencyRateDownloader;

        public CurrencyRateDownloaderTests(TestCurrenciesAppFactory factory, TestCurrenciesDbContext dbContext) : base(factory, dbContext)
        {
            _wireMockServer = WireMockServer;
            _currencyRateDownloader = Factory.Services.GetRequiredService<ISchedulerTask<CurrencyRateScheduler>>();
        }
    }
}
