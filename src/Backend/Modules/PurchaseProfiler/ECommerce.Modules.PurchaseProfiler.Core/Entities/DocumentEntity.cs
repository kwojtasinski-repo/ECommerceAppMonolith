using Newtonsoft.Json;

namespace ECommerce.Modules.PurchaseProfiler.Core.Entities
{
    public class DocumentEntity : IDocumentEntity<long>
    {
        [JsonProperty("_id")]
        public string? Id { get; set; }
        [JsonProperty("_key")]
        public string? Key { get; set; }

        [JsonIgnore]
        public long KeyValue => GetKeyValue();

        private long _keyParsed;

        private long GetKeyValue()
        {
            if (_keyParsed > 0)
            {
                return _keyParsed;
            }

            if (long.TryParse(Key, out _keyParsed))
            { 
                return _keyParsed;
            }

            throw new ArgumentException(string.Format("Key '{0}' not type of long", KeyValue));
        }
    }
}
