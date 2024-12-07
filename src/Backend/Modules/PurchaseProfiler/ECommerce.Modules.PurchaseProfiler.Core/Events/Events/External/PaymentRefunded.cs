using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.PurchaseProfiler.Core.Events.Events.External
{
    public record PaymentRefunded(Guid PaymentId, Guid OrderId) : IEvent;
}
