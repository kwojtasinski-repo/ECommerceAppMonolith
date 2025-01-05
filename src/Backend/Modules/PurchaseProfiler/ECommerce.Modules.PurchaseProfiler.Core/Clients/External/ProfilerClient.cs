using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace ECommerce.Modules.PurchaseProfiler.Core.Clients.External
{
    internal class ProfilerClient
        (
            IHttpClientFactory httpClientFactory,
            ILogger<ProfilerClient> logger
        )
        : IProfilerClient
    {
        public async Task<PredictionResponse?> PredictPurchases(PredictionRequest request)
        {
            var client = httpClientFactory.CreateClient("ProfilerClient");
            var response = await client.PostAsJsonAsync("/predict", request, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });
            logger.LogInformation("{methodName}: Sending request for prediction purchases", nameof(PredictPurchases));

            if (!response.IsSuccessStatusCode)
            {
                logger.LogInformation("{methodName}: Sending request failed, status: '{statusCode}'", nameof(PredictPurchases), response.StatusCode);
                return null;
            }

            logger.LogInformation("{methodName}: Sending request success, received purchases", nameof(PredictPurchases));
            return await response.Content.ReadFromJsonAsync<PredictionResponse>(new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });
        }
    }
}
