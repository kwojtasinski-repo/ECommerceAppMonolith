using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Shared.Abstractions.Queries;

namespace ECommerce.Modules.Items.Application.Queries.ItemSales
{
    public record GetAllFilteredByName(string Name) : IQuery<IEnumerable<ItemSaleDto>>;
}
