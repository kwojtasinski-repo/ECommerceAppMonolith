using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.Items.Application.Commands.ItemSales
{
    public record ItemSaleUpdated(Guid ItemSaleId, decimal ItemCost, string CurrencyCode) : IEvent;
}
