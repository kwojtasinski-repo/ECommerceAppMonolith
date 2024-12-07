using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.PurchaseProfiler.Core.Events.External
{
    public record ItemSaleUpdated(Guid ItemSaleId, decimal ItemCost, string CurrencyCode) : IEvent;
}
