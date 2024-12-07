using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.PurchaseProfiler.Core.Events.External
{
    public record ItemSaleAdded(Guid Id, Guid ItemId, decimal Cost, string CurrencyCode) : IEvent;
}
