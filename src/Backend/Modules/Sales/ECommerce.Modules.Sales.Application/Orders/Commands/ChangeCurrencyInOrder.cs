using ECommerce.Shared.Abstractions.Commands;

namespace ECommerce.Modules.Sales.Application.Orders.Commands
{
    public record ChangeCurrencyInOrder(Guid OrderId, string CurrencyCode) : ICommand;
}
