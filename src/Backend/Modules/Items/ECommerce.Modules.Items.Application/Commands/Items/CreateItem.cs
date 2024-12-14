using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Shared.Abstractions.Commands;

namespace ECommerce.Modules.Items.Application.Commands.Items
{
    public record CreateItem(string ItemName, string? Description, Guid BrandId, Guid TypeId, IEnumerable<string>? Tags, IEnumerable<ImageUrl>? ImagesUrl) : ICommand
    {
        public Guid ItemId { get; } = Guid.NewGuid();
    };
}
