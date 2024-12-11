using ECommerce.Modules.PurchaseProfiler.Core.Services;

namespace ECommerce.Modules.PurchaseProfiler.Core.Entities
{
    public class UserModel : DocumentEntity, IDocumentEntity<long>
    {
        public override string CollectionName => "userModels";

        public Guid UserId { get; set; }
        public int ModelVersion { get; set; }
        public DateTime CreateDate { get; set; }
        public string ModelData { get; set; } = string.Empty;
        public long Version { get; set; }
        public IEnumerable<CustomerData> TrainingDataSet { get; set; } = [];
    }

}
