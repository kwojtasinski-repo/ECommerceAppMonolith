namespace ECommerce.Modules.PurchaseProfiler.Core.Entities
{
    public class UserModel : DocumentEntity, IDocumentEntity<long>
    {
        public string UserId { get; set; }
        public string ModelId { get; set; }
        public int ModelVersion { get; set; }
        public DateTime LastUpdated { get; set; }
        public string ModelBase64 { get; set; }
        public string ModelMetadata { get; set; }
    }

}
