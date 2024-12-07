using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.PurchaseProfiler.Core.Events.External
{
    public record ItemSaleActivated(Guid Id) : IEvent;
}
