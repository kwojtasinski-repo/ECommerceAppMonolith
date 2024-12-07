using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.Items.Application.Events
{
    public record BrandNameChanged(Guid Id, string Name) : IEvent;
}
