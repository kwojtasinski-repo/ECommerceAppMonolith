using ECommerce.Modules.Currencies.Core.Exceptions;
using Flurl.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Core.Clients
{
    internal class NbpClient : INbpClient
    {
        private readonly IFlurlClient _flurlClient;
        private readonly ClientOptions _clientOptions;

        public NbpClient(IFlurlClient flurlClient, IOptions<ClientOptions> clientOptions)
        {
            _flurlClient = flurlClient;
            _clientOptions = clientOptions.Value;
        }

        public async Task<string> GetCurrency(string currencyCode)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(_clientOptions.BaseUrl != null ? _clientOptions.BaseUrl.TrimEnd('/') : "").Append("/api/exchangerates/rates/a/");
            urlBuilder.Append(currencyCode.ToLower());
            var content = await SendGetRequest(urlBuilder.ToString());
            return content;
        }

        public async Task<string> GetCurrencyRateOnDate(string currencyCode, DateTime dateTime)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(_clientOptions.BaseUrl != null ? _clientOptions.BaseUrl.TrimEnd('/') : "").Append("/api/exchangerates/rates/a/");
            urlBuilder.Append(currencyCode.ToLower()).Append("/");
            urlBuilder.Append(dateTime.ToString("yyyy-MM-dd"));
            var content = await SendGetRequest(urlBuilder.ToString());
            return content;
        }

        private async Task<string> SendGetRequest(string urlBuilder)
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
                        var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        return responseData;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return null;
                    }
                    else
                    {
                        throw new InvalidUrlException();
                    }
                }
            }
            catch (Exception exception)
            {
                throw new InvalidUrlException(exception);
            }
        }
    }
}
