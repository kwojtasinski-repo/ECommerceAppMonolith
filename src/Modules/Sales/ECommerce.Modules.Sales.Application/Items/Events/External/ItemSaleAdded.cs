using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.Sales.Application.Items.Events.External
{
    public record ItemSaleAdded(Guid Id, Guid ItemId, decimal Cost) : IEvent;
}
