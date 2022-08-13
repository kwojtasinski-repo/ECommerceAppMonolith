using ECommerce.Shared.Abstractions.Commands;

namespace ECommerce.Modules.Sales.Application.Payments.Commands
{
    public record DeletePayment(Guid PaymentId) : ICommand;
}
