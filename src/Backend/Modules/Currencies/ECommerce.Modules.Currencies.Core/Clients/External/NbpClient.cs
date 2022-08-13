using Flurl.Http;
using Microsoft.Extensions.Options;
using System.Text;

namespace ECommerce.Modules.Currencies.Core.Clients.External
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

        public async Task<IEnumerable<ExchangeRateTable>> GetAllCurrenciesForCurrentDay(CancellationToken cancellationToken)
        {
            var urlBuilder = new StringBuilder();
            var baseUrl = _clientOptions.BaseUrl;
            urlBuilder.Append(baseUrl != null ? baseUrl.TrimEnd('/') : "").Append("/api/exchangerates/tables/a");
            var content = await SendGetRequest<IEnumerable<ExchangeRateTable>>(urlBuilder.ToString(), cancellationToken);
            return content ?? new List<ExchangeRateTable>();
        }

        public async Task<ExchangeRate> GetCurrencyAsync(string currencyCode)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(_clientOptions.BaseUrl != null ? _clientOptions.BaseUrl.TrimEnd('/') : "").Append("/api/exchangerates/rates/a/");
            urlBuilder.Append(currencyCode.ToLower());
            var content = await SendGetRequest<ExchangeRate>(urlBuilder.ToString());
            return content;
        }

        public async Task<ExchangeRate> GetCurrencyRateOnDateAsync(string currencyCode, DateOnly dateTime)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(_clientOptions.BaseUrl != null ? _clientOptions.BaseUrl.TrimEnd('/') : "").Append("/api/exchangerates/rates/a/");
            urlBuilder.Append(currencyCode.ToLower()).Append("/");
            urlBuilder.Append(dateTime.ToString("yyyy-MM-dd"));
            var content = await SendGetRequest<ExchangeRate>(urlBuilder.ToString());
            return content;
        }

        private async Task<T> SendGetRequest<T>(string urlBuilder)
            where T : class
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(_flurlClient.Settings.Timeout.Value);
            return await SendGetRequest<T>(urlBuilder, cancellationTokenSource.Token);
        }
        
        private async Task<T> SendGetRequest<T>(string urlBuilder, CancellationToken cancellationToken)
            where T : class
        {
            try
            {
                var response = await _flurlClient.Request(urlBuilder)
                    .AllowHttpStatus("404")
                    .GetAsync(cancellationToken: cancellationToken, completionOption: HttpCompletionOption.ResponseHeadersRead);
                
                if (response.StatusCode == (int) System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                var exchangeRate = await response.GetJsonAsync<T>();
                return exchangeRate;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
