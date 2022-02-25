using ECommerce.Shared.Abstractions.Commands;

namespace ECommerce.Modules.Sales.Application.Payments.Commands
{
    public record CreatePayment(Guid OrderId, Guid UserId) : ICommand
    {
        public Guid Id = Guid.NewGuid();
    };
}
