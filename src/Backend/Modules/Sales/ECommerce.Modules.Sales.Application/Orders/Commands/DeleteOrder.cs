using ECommerce.Shared.Abstractions.Commands;

namespace ECommerce.Modules.Sales.Application.Orders.Commands
{
    public record DeleteOrder(Guid OrderId) : ICommand;
}
