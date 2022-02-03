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

        public NbpClient(HttpClient httpClient, IOptions<NbpClientOptions> clientOptions)
        {
            _flurlClient = new FlurlClient(httpClient);
            _clientOptions = clientOptions.Value;
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
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    var url = urlBuilder;
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);
                    var response = await _flurlClient.HttpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var responseData = response.Content == null ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        var exchangeRate = string.IsNullOrEmpty(responseData) ? null : JsonSerializer.Deserialize<ExchangeRate>(responseData);
                        return exchangeRate;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return null;
                    }
                    else if ((int) response.StatusCode >= 400 && (int) response.StatusCode < 500)
                    {
                        throw new InvalidUrlException(url);
                    }
                    else if((int) response.StatusCode >= 500 && (int) response.StatusCode < 600)
                    {
                        throw new ServerNotAvailableException(url);
                    }
                    else
                    {
                        throw new InvalidUrlException(url);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
