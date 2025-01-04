using System.Net.Http.Json;
using System.Text.Json;

namespace ECommerce.Modules.PurchaseProfiler.Core.External
{
    internal class ProfilerClient
        (
            IHttpClientFactory httpClientFactory
        )
        : IProfilerClient
    {

        public async Task<PredictionResponse?> PredictPurchases(PredictionRequest request)
        {
            var client = httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("/predict", request, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });

            if (response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<PredictionResponse>(new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });
        }
    }
}
