using ECommerce.Shared.Abstractions.Commands;

namespace ECommerce.Modules.Sales.Application.Orders.Commands
{
    public record CreateOrderItem(Guid ItemSaleId, Guid UserId) : ICommand
    {
        public Guid OrderItem = Guid.NewGuid();
    };
}
