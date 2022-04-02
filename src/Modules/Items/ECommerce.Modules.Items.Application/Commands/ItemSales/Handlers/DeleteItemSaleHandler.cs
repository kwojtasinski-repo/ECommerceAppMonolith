using ECommerce.Modules.Items.Application.Exceptions;
using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Shared.Abstractions.Commands;

namespace ECommerce.Modules.Items.Application.Commands.ItemSales.Handlers
{
    internal class DeleteItemSaleHandler : ICommandHandler<DeleteItemSale>
    {
        private readonly IItemSaleRepository _itemSaleRepository;

        public DeleteItemSaleHandler(IItemSaleRepository itemSaleRepository)
        {
            _itemSaleRepository = itemSaleRepository;
        }

        public async Task HandleAsync(DeleteItemSale command)
        {
            var itemSale = await _itemSaleRepository.GetAsync(command.ItemSaleId);

            if (itemSale is null)
            {
                throw new ItemSaleNotFoundException(command.ItemSaleId);
            }

            await _itemSaleRepository.DeleteAsync(itemSale);
        }
    }
}
