using ECommerce.Modules.Items.Application.DTO;
using ECommerce.Modules.Items.Application.Mappings;
using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Shared.Abstractions.Commands;

namespace ECommerce.Modules.Items.Application.Commands.Items.Handlers
{
    internal class GetProductDataHandler
        (
            IItemRepository itemRepository
        )
        : ICommandHandler<GetProductData, ProductDataDto?>
    {
        public async Task<ProductDataDto?> HandleAsync(GetProductData command)
        {
            var product = await itemRepository.GetProductDataAsync(command.ProductId);
            return product?.AsProductDataDto();
        }
    }
}
