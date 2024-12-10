namespace ECommerce.Modules.PurchaseProfiler.Core.Database
{
    public class CollectionInfo
    {
        public Type? CollectionType { get; set; }
        public string CollectionName { get; set; } = string.Empty;
        public KeyGenerationType KeyGenerationType { get; set; } = KeyGenerationType.Autoincrement;
    }
}
