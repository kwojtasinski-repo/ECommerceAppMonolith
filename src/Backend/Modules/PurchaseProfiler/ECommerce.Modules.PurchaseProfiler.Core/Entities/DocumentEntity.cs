using Newtonsoft.Json;

namespace ECommerce.Modules.PurchaseProfiler.Core.Entities
{
    public class DocumentEntity : IDocumentEntity<long>
    {
        [JsonIgnore]
        public string Id => _id ?? string.Empty;
        [JsonIgnore]
        public string Key => _key ?? string.Empty;
        [JsonIgnore]
        public long KeyValue => GetKeyValue();

        [JsonProperty]
        private string? _id;
        
        [JsonProperty]
        private string? _key;

        private long _keyParsed;

        public DocumentEntity() : this(null, null)
        { }

        public DocumentEntity(string? id = null, string? key = null)
        {
            _id = id;
            _key = key;
        }

        public void SetId(string? id)
        {
            _id = id;
        }

        public void SetKey(string? key)
        {
            _key = key;
        }

        private long GetKeyValue()
        {
            if (_keyParsed > 0)
            {
                return _keyParsed;
            }

            if (long.TryParse(_key, out _keyParsed))
            { 
                return _keyParsed;
            }

            throw new ArgumentException(string.Format("Key '{0}' not type of long", KeyValue));
        }
    }
}
