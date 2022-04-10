using ECommerce.Shared.Abstractions.Commands;

namespace ECommerce.Modules.Sales.Application.Orders.Commands
{
    public record CreateOrderItems(IEnumerable<Guid> ItemSaleIds, string CurrencyCode) : ICommand;
}
