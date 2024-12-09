namespace ECommerce.Modules.PurchaseProfiler.Core.Entities
{
    internal interface IDocumentEntity<T>
        where T : struct
    {
        public string? Id { get; }
        public T KeyValue { get; }
        public string? Key { get; }
    }
}
