using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.Items.Application.Events
{
    public record TypeNameChanged(Guid Id, string Name) : IEvent;
}
