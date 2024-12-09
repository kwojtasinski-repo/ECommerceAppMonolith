namespace ECommerce.Modules.PurchaseProfiler.Core.Entities
{
    internal interface IDocumentEntity<T>
        where T : struct
    {
        public string Id { get; }
        public T KeyValue { get; }
        public string Key { get; }

        public void SetId(string? id);
        public void SetKey(string? key);
    }
}
