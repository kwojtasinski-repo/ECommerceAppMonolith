using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.Sales.Application.Items.Events.External
{
    public record ItemUpdated(Guid Id, string ItemName, string? Description, string BrandName, string TypeName, IEnumerable<string>? Tags, IEnumerable<string>? ImagesUrl) : IEvent;
}