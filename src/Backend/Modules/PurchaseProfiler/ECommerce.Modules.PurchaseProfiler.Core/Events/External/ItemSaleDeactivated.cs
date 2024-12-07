using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.PurchaseProfiler.Core.Events.External
{
    public record ItemSaleDeactivated(Guid Id) : IEvent;
}
