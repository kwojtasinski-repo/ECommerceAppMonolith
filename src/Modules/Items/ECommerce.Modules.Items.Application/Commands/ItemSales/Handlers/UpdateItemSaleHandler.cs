using ECommerce.Modules.Items.Application.Exceptions;
using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Shared.Abstractions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Items.Application.Commands.ItemSales.Handlers
{
    internal class UpdateItemSaleHandler : ICommandHandler<UpdateItemSale>
    {
        private readonly IItemSaleRepository _itemSaleRepository;

        public UpdateItemSaleHandler(IItemSaleRepository itemSaleRepository)
        {
            _itemSaleRepository = itemSaleRepository;
        }

        public async Task HandleAsync(UpdateItemSale command)
        {
            Validate(command);
            var itemSale = await _itemSaleRepository.GetAsync(command.ItemSaleId);

            if (itemSale is null)
            {
                throw new ItemSaleNotFoundException(command.ItemSaleId);
            }

            itemSale.ChangeCost(command.ItemCost);
            await _itemSaleRepository.UpdateAsync(itemSale);
        }

        private static void Validate(UpdateItemSale command)
        {
            if (command is null)
            {
                throw new UpdateItemSaleCannotBeNullException();
            }

            if (command.ItemCost < 0)
            {
                throw new ItemCostCannotBeNegativeException(command.ItemCost);
            }
        }
    }
}
