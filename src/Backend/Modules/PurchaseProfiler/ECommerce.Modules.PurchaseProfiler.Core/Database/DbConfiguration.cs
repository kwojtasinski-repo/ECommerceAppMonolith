namespace ECommerce.Modules.PurchaseProfiler.Core.Database
{
    internal class DbConfiguration
    {
        public IEnumerable<string> DatabaseNames = [];
        public IEnumerable<CollectionInfo> Collections = [];

        public string GetCollectionName(Type type)
        {
            return Collections.FirstOrDefault(c => c.CollectionType == type)
                              ?.CollectionName ?? string.Empty;
        }
    }
}
