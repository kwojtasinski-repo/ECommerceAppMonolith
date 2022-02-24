using ECommerce.Shared.Abstractions.Commands;

namespace ECommerce.Modules.Sales.Application.Orders.Commands
{
    public record CreateOrder(Guid CustomerId, Guid UserId) : ICommand
    {
        public Guid Id = Guid.NewGuid();
    };
}
