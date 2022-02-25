using ECommerce.Shared.Abstractions.Commands;

namespace ECommerce.Modules.Sales.Application.Orders.Commands
{
    public record CreateOrder(Guid CustomerId) : ICommand
    {
        public Guid Id = Guid.NewGuid();
    };
}
