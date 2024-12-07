using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.Sales.Application.Payments.Events
{
    public record PaymentRefunded(Guid PaymentId, Guid OrderId) : IEvent;
}
