using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.Sales.Application.Items.Events.External
{
    public record ItemSaleCostChanged(Guid Id, decimal Cost) : IEvent;
}
