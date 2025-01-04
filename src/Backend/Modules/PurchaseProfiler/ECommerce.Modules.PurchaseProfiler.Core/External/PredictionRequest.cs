using System.Text.Json;
using System.Text.Json.Serialization;

namespace ECommerce.Modules.PurchaseProfiler.Core.External
{
    internal class PredictionRequest
    {
        public List<List<long>> PurchaseHistory { get; set; } = [];

        [JsonConverter(typeof(ProductFrequencyConverter))]
        public List<List<ProductFrequency>> ProductFrequencies { get; set; } = [];

        public List<long> Labels { get; set; } = [];

        public bool Latest { get; set; } = true;

        public int? TopK { get; set; }
    }

    internal class ProductFrequency
    {
        public string ProductId { get; set; } = string.Empty;

        public int Quantity { get; set; }
    }

    internal class ProductFrequencyConverter : JsonConverter<List<List<ProductFrequency>>>
    {
        public override List<List<ProductFrequency>>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dictionaryList = JsonSerializer.Deserialize<List<Dictionary<string, int>>>(ref reader, options);
            if (dictionaryList is null)
            {
                return [];
            }

            var productFrequenciesList = new List<List<ProductFrequency>>();
            foreach (var dictionary in dictionaryList)
            {
                var productFrequencies = new List<ProductFrequency>();

                foreach (var entry in dictionary)
                {
                    productFrequencies.Add(new ProductFrequency
                    {
                        ProductId = entry.Key,
                        Quantity = entry.Value
                    });
                }

                productFrequenciesList.Add(productFrequencies);
            }

            return productFrequenciesList;
        }

        public override void Write(Utf8JsonWriter writer, List<List<ProductFrequency>> value, JsonSerializerOptions options)
        {
            var dictionaryList = new List<Dictionary<string, int>>();

            foreach (var items in value)
            {
                var dictionary = new Dictionary<string, int>();

                foreach (var item in items)
                {
                    dictionary[item.ProductId] = item.Quantity;
                }

                dictionaryList.Add(dictionary);
            }

            JsonSerializer.Serialize(writer, dictionaryList, options);
        }
    }
}
