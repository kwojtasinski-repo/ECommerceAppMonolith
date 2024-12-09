using ECommerce.Modules.PurchaseProfiler.Core.Database;
using Newtonsoft.Json;

namespace ECommerce.Modules.PurchaseProfiler.Core.Entities
{
    [KeyGenerationType(KeyGenerationType.Autoincrement)]
    public abstract class DocumentEntity : IDocumentEntity<long>
    {
        public abstract string CollectionName { get; }

        [JsonProperty("_id")]
        public string? Id { get; set; }

        [JsonProperty("_key")]
        public string? Key { get; set; }

        [JsonIgnore]
        public long KeyValue => GetKeyValue();

        private long _keyParsed;

        protected DocumentEntity()
        { }

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
