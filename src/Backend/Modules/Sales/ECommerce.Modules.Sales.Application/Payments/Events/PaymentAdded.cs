using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.Sales.Application.Payments.Events
{
    public record PaymentAdded(Guid PaymentId, Guid OrderId, DateTime PaymentDate) : IEvent;
}
