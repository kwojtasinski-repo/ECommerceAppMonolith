using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Shared.Abstractions.Queries;

namespace ECommerce.Modules.Items.Application.Queries.Items
{
    public record GetItemsNotPutUpForSale : IQuery<IEnumerable<ItemDto>>;
}
