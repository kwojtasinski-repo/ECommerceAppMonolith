using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.Sales.Application.Items.Events.External
{
    public record ItemDescriptionChanged(Guid Id, string Description) : IEvent;
}
