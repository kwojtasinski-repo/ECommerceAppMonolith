using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.Items.Application.Events
{
    public record ItemAdded(Guid Id, string ItemName, string? Description, string BrandName, string TypeName, IEnumerable<string>? Tags, IEnumerable<string>? ImagesUrl) : IEvent;
}
