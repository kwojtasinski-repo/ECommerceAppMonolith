using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.Items.Application.Events
{
    public record ItemSaleDeactivated(Guid Id) : IEvent;
}
