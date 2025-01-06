using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Mappings;
using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Shared.Abstractions.Commands;

namespace ECommerce.Modules.Items.Application.Commands.Items.Handlers
{
    internal class GetProductsDetailsHandler
        (
            IItemRepository itemRepository
        )
        : ICommandHandler<GetProductsDataDetails, ProductsDataDetailsDto?>
    {
        public async Task<ProductsDataDetailsDto?> HandleAsync(GetProductsDataDetails command)
        {
            if (command.ProductIds is null || !command.ProductIds.Any())
            {
                return new ProductsDataDetailsDto([]);
            }

            return new ProductsDataDetailsDto(
                (await itemRepository.GetProductsDataDetailsAsync(command.ProductIds))
                .Select(i => i.AsProductsDataDetailsDto())
                .ToList()
            );
        }
    }
}
