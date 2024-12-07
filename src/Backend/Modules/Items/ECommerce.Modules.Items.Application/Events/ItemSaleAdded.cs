using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.Items.Application.Events
{
    public record ItemSaleAdded(Guid Id, Guid ItemId, decimal Cost, string CurrencyCode) : IEvent;
}
