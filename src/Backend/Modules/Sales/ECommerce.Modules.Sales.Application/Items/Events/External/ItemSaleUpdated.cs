using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.Sales.Application.Items.Events.External
{
    public record ItemSaleUpdated(Guid ItemSaleId, decimal ItemCost, string CurrencyCode) : IEvent;
}
