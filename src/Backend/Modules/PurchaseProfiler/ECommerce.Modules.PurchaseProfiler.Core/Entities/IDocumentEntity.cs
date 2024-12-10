namespace ECommerce.Modules.PurchaseProfiler.Core.Entities
{
    internal interface IDocumentEntity<T>
        where T : struct
    {
        public string CollectionName { get; }
        public string? Id { get; set; }
        public T KeyValue { get; }
        public string? Key { get; set; }
    }
}
