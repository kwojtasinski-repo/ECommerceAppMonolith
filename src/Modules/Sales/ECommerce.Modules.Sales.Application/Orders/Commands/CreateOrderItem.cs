using ECommerce.Shared.Abstractions.Commands;

namespace ECommerce.Modules.Sales.Application.Orders.Commands
{
    public record CreateOrderItem(Guid ItemSaleId) : ICommand
    {
        public Guid OrderItemId = Guid.NewGuid();
    };
}
