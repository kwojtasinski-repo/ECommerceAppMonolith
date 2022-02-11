using ECommerce.Modules.Currencies.Core.Exceptions;
using Flurl.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Core.Clients
{
    internal class NbpClient : INbpClient
    {
        private readonly IFlurlClient _flurlClient;
        private readonly NbpClientOptions _clientOptions;

        public NbpClient(IHttpClientFactory httpClient, IOptions<NbpClientOptions> clientOptions)
        {
            _flurlClient = new FlurlClient(httpClient.CreateClient());
            _clientOptions = clientOptions.Value;
            _flurlClient.WithTimeout(_clientOptions.Timeout);
        }

        public async Task<ExchangeRate> GetCurrencyAsync(string currencyCode)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(_clientOptions.BaseUrl != null ? _clientOptions.BaseUrl.TrimEnd('/') : "").Append("/api/exchangerates/rates/a/");
            urlBuilder.Append(currencyCode.ToLower());
            var content = await SendGetRequest(urlBuilder.ToString());
            return content;
        }

        public async Task<ExchangeRate> GetCurrencyRateOnDateAsync(string currencyCode, DateOnly dateTime)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(_clientOptions.BaseUrl != null ? _clientOptions.BaseUrl.TrimEnd('/') : "").Append("/api/exchangerates/rates/a/");
            urlBuilder.Append(currencyCode.ToLower()).Append("/");
            urlBuilder.Append(dateTime.ToString("yyyy-MM-dd"));
            var content = await SendGetRequest(urlBuilder.ToString());
            return content;
        }

        private async Task<ExchangeRate> SendGetRequest(string urlBuilder)
        {
            try
            {
                using var cancellationTokenSource = new CancellationTokenSource();
                cancellationTokenSource.CancelAfter(_flurlClient.Settings.Timeout.Value);
                var response = await _flurlClient.Request(urlBuilder).GetAsync(cancellationToken: cancellationTokenSource.Token, completionOption: HttpCompletionOption.ResponseHeadersRead);

                var exchangeRate = await response.GetJsonAsync<ExchangeRate>();
                return exchangeRate;
            }
            catch (FlurlHttpException exception)
            {
                if (exception.StatusCode == (int) System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw exception;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
