using ECommerce.Shared.Abstractions.Commands;

namespace ECommerce.Modules.Sales.Application.Orders.Commands
{
    public record ChangeCustomerInOrder(Guid OrderId, Guid CustomerId) : ICommand;
}
