using ECommerce.Modules.Currencies.Core.Clients.External;
using ECommerce.Modules.Currencies.Core.DTO;
using ECommerce.Modules.Currencies.Core.Scheduler;
using ECommerce.Modules.Currencies.Core.Services;
using ECommerce.Shared.Abstractions.Time;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Modules.Currencies.Tests.Unit.SchedulerJobs
{
    public class CurrencyRateDownloaderTests
    {
        [Fact]
        public async Task should_add_currency_rates()
        {
            var token = CancellationToken.None;
            var currenciesFromApi = GetAllCurrenciesFromApi();
            _nbpClient.GetAllCurrenciesForCurrentDay(token).Returns(currenciesFromApi);
            var currencies = GetAllCurrencies();
            _currencyService.GetAllAsync().Returns(currencies);
            _currencyRateService.GetCurrencyRatesForDate(Arg.Any<IEnumerable<string>>(), DateOnly.FromDateTime(DateTime.UtcNow))
                .Returns(new List<CurrencyRateDto> {
                    new CurrencyRateDto { Id = Guid.NewGuid(), CurrencyDate = DateOnly.FromDateTime(DateTime.UtcNow), CurrencyId = new Guid(), Rate = 1 } });

            await _rateDownloader.DoWork(token);

            await _currencyRateService.Received(2).AddAsync(Arg.Any<CurrencyRateDto>());
        }

        [Fact]
        public async Task should_add_and_update_currency_rates()
        {
            var token = CancellationToken.None;
            var currenciesFromApi = GetAllCurrenciesFromApi();
            _nbpClient.GetAllCurrenciesForCurrentDay(token).Returns(currenciesFromApi);
            var currencies = GetAllCurrencies();
            _currencyService.GetAllAsync().Returns(currencies);
            _currencyRateService.GetCurrencyRatesForDate(Arg.Any<IEnumerable<string>>(), DateOnly.FromDateTime(DateTime.UtcNow))
                .Returns(new List<CurrencyRateDto> {
                    new CurrencyRateDto { Id = Guid.NewGuid(), CurrencyDate = DateOnly.FromDateTime(DateTime.UtcNow), CurrencyId = new Guid(), Rate = 1.2M } });

            await _rateDownloader.DoWork(token);

            await _currencyRateService.Received(2).AddAsync(Arg.Any<CurrencyRateDto>());
            await _currencyRateService.Received(1).UpdateAsync(Arg.Any<CurrencyRateDto>());
        }

        [Fact]
        public async Task should_update_currency_rates()
        {
            var token = CancellationToken.None;
            var currenciesFromApi = GetAllCurrenciesFromApi();
            _nbpClient.GetAllCurrenciesForCurrentDay(token).Returns(currenciesFromApi);
            var currencies = GetAllCurrencies();
            _currencyService.GetAllAsync().Returns(currencies);
            _currencyRateService.GetCurrencyRatesForDate(Arg.Any<IEnumerable<string>>(), DateOnly.FromDateTime(DateTime.UtcNow))
                .Returns(new List<CurrencyRateDto> {
                    new CurrencyRateDto { Id = Guid.NewGuid(), CurrencyDate = DateOnly.FromDateTime(DateTime.UtcNow), CurrencyId = new Guid(), Rate = 1.2M },
                    new CurrencyRateDto { Id = Guid.NewGuid(), CurrencyDate = DateOnly.FromDateTime(DateTime.UtcNow), CurrencyId = currencies.Where(c => c.Code == "EUR").FirstOrDefault().Id, Rate = 5.2M },
                    new CurrencyRateDto { Id = Guid.NewGuid(), CurrencyDate = DateOnly.FromDateTime(DateTime.UtcNow), CurrencyId = currencies.Where(c => c.Code == "USD").FirstOrDefault().Id, Rate = 4.2M }});

            await _rateDownloader.DoWork(token);

            await _currencyRateService.Received(3).UpdateAsync(Arg.Any<CurrencyRateDto>());
        }
        
        private List<CurrencyDto> GetAllCurrencies()
        {
            var currencies = new List<CurrencyDto>();
            currencies.Add(new CurrencyDto() { Id = Guid.NewGuid(), Code = "EUR", Description = "Euro" });
            currencies.Add(new CurrencyDto() { Id = Guid.NewGuid(), Code = "USD", Description = "Dolar" });
            currencies.Add(new CurrencyDto() { Id = new Guid(), Code = "PLN", Description = "Polski zloty" });
            return currencies;
        }

        private List<ExchangeRateTable> GetAllCurrenciesFromApi()
        {
            var exchangeRateTable = new ExchangeRateTable()
            {
                EffectiveDate = DateTime.UtcNow,
                No = "Nr 1234",
                Table = "A",
                Rates = new System.Collections.Generic.List<RateTable>
                {
                    new RateTable { Code = "EUR", Currency = "Euro", Mid = 4M },
                    new RateTable { Code = "USD", Currency = "Dolar", Mid = 2M }
                }
            };

            return new List<ExchangeRateTable> { exchangeRateTable };
        }

        private readonly CurrencyRateDownloader _rateDownloader;
        private readonly ILogger<CurrencyRateDownloader> _logger;
        private readonly ICurrencyService _currencyService;
        private readonly ICurrencyRateService _currencyRateService;
        private readonly INbpClient _nbpClient;
        private readonly IClock _clock;

        public CurrencyRateDownloaderTests()
        {
            _logger = Substitute.For<ILogger<CurrencyRateDownloader>>();
            _currencyService = Substitute.For<ICurrencyService>();
            _currencyRateService = Substitute.For<ICurrencyRateService>();
            _nbpClient = Substitute.For<INbpClient>();
            _clock = Substitute.For<IClock>();
            _clock.CurrentDate().Returns(DateTime.UtcNow);
            _rateDownloader = new CurrencyRateDownloader(_logger, _currencyService, _currencyRateService,
                                        _nbpClient, _clock);
        }
    }
}
