using ECommerce.Modules.Currencies.Core.Clients;
using ECommerce.Modules.Currencies.Tests.Unit.Common;
using Flurl.Http;
using Flurl.Http.Testing;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Modules.Currencies.Tests.Unit.Clients
{
    public class NbpClientTests
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly IOptions<NbpClientOptions> _clientOptions;

        public NbpClientTests()
        {
            _httpClient = Substitute.For<IHttpClientFactory>();
            _clientOptions = Substitute.For<IOptions<NbpClientOptions>>();
            _clientOptions.Value.Returns(new NbpClientOptions() { BaseUrl = "http://localhost", Timeout = 10 });
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new DateOnlyJsonConverter() }
            };
        }

        [Fact]
        public async Task given_valid_currency_code_should_return_exchange_rate()
        {
            var content = GetContent();
            var clientHandlerStub = new DelegatingHandlerStub((request, cancellationToken) => {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(content);
                return Task.FromResult(response);
            });
            var client = new HttpClient(clientHandlerStub);
            _httpClient.CreateClient().Returns(client);
            var nbpClient = new NbpClient(_httpClient, _clientOptions);
            var code = "eur";
            var rate = 4.5163M;

            var exchangeRate = await nbpClient.GetCurrencyAsync(code);

            exchangeRate.ShouldNotBeNull();
            exchangeRate.Code.ToLower().ShouldBe(code.ToLower());
            exchangeRate.Rates.FirstOrDefault().ShouldNotBeNull();
            exchangeRate.Rates.FirstOrDefault().Mid.ShouldBe(rate);
        }

        [Fact]
        public async Task given_valid_currency_code_when_not_existing_rate_should_return_null()
        {
            var content = GetContent();
            var clientHandlerStub = new DelegatingHandlerStub((request, cancellationToken) => {
                var response = new HttpResponseMessage(HttpStatusCode.NotFound);
                return Task.FromResult(response);
            });
            var client = new HttpClient(clientHandlerStub);
            _httpClient.CreateClient().Returns(client);
            var nbpClient = new NbpClient(_httpClient, _clientOptions);
            var code = "eur";

            var exchangeRate = await nbpClient.GetCurrencyAsync(code);

            exchangeRate.ShouldBeNull();
        }

        [Fact]
        public async Task given_valid_currency_code_and_not_available_server_should_throw_an_exception()
        {
            var clientHandlerStub = new DelegatingHandlerStub((request, cancellationToken) => {
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                return Task.FromResult(response);
            });
            var client = new HttpClient(clientHandlerStub);
            _httpClient.CreateClient().Returns(client);
            var nbpClient = new NbpClient(_httpClient, _clientOptions);
            var code = "eur";

            var exception = await Record.ExceptionAsync(() => nbpClient.GetCurrencyAsync(code));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<FlurlHttpException>();
            ((FlurlHttpException) exception).StatusCode.ShouldBe((int) HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task given_valid_currency_when_timeout_should_throw_an_exception()
        {
            var clientHandlerStub = new DelegatingHandlerStub((request, cancellationToken) => {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                throw new TimeoutException();
            });
            var client = new HttpClient(clientHandlerStub);
            _httpClient.CreateClient().Returns(client);
            var nbpClient = new NbpClient(_httpClient, _clientOptions);
            var code = "eur";

            var exception = await Record.ExceptionAsync(() => nbpClient.GetCurrencyAsync(code));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<FlurlHttpException>();
        }

        private string GetContent(DateOnly date = new DateOnly())
        {
            var defaultDate = date == new DateOnly() ? new DateOnly(2022, 2, 11) : date;
            var defaultDateString = defaultDate.ToString("yyyy-MM-dd");
            return "{\"table\":\"A\",\"currency\":\"euro\",\"code\":\"EUR\",\"rates\":[{\"no\":\"029/A/NBP/2022\",\"effectiveDate\":" + $"\"{defaultDateString}" + "\",\"mid\":4.5163}]}";
        }
    }
}
