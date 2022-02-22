using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.Sales.Application.Items.Events.External
{
    public record ItemNameChanged(Guid Id, string ItemName) : IEvent;
}
