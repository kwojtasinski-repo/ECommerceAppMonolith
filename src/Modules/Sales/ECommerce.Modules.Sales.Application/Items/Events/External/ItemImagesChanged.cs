using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.Sales.Application.Items.Events.External
{
    public record ItemImagesChanged(Guid Id, IEnumerable<string> Images) : IEvent;
}
