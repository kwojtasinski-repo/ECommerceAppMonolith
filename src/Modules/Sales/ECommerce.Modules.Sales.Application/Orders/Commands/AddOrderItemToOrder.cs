using ECommerce.Shared.Abstractions.Commands;

namespace ECommerce.Modules.Sales.Application.Orders.Commands
{
    public record AddOrderItemToOrder(Guid OrderId, Guid ItemSaleId, Guid UserId) : ICommand;
}
