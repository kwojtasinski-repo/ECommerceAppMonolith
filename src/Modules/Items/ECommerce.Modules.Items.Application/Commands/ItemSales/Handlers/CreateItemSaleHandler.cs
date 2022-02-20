using ECommerce.Modules.Items.Application.Exceptions;
using ECommerce.Modules.Items.Domain.Entities;
using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Shared.Abstractions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Commands.ItemSales.Handlers
{
    internal class CreateItemSaleHandler : ICommandHandler<CreateItemSale>
    {
        private readonly IItemSaleRepository _itemSaleRepository;
        private readonly IItemRepository _itemRepository;

        public CreateItemSaleHandler(IItemSaleRepository itemSaleRepository, IItemRepository itemRepository)
        {
            _itemSaleRepository = itemSaleRepository;
            _itemRepository = itemRepository;
        }

        public async Task HandleAsync(CreateItemSale command)
        {
            Validate(command);
            var item = await _itemRepository.GetAsync(command.ItemId);

            if (item is null)
            {
                throw new ItemNotFoundException(command.ItemId);
            }

            if (item.ImagesUrl is null)
            {
                throw new CannotCreateItemSaleWithoutImagesException();
            }

            item.ImagesUrl.TryGetValue(Item.IMAGES, out var images);
            if (images.Where(i => i.MainImage == true).SingleOrDefault() == null)
            {
                throw new CannotCreateItemSaleWithoutMainImageException();
            }

            var itemSale = ItemSale.Create(command.ItemSaleId, item, command.ItemCost);
            await _itemSaleRepository.AddAsync(itemSale);
        }

        private static void Validate(CreateItemSale command)
        {
            if (command is null)
            {
                throw new CreateItemSaleCannotBeNullException();
            }

            if (command.ItemCost < 0)
            {
                throw new ItemCostCannotBeNegativeException(command.ItemCost);
            }
        }
    }
}
