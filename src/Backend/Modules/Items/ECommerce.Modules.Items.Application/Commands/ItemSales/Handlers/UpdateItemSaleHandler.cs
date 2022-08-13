using ECommerce.Modules.Items.Application.Exceptions;
using ECommerce.Modules.Items.Application.Services;
using ECommerce.Modules.Items.Domain.Repositories;
using ECommerce.Shared.Abstractions.Commands;
using ECommerce.Shared.Abstractions.Messagging;
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
        private readonly IMessageBroker _messageBroker;

        public UpdateItemSaleHandler(IItemSaleRepository itemSaleRepository, IMessageBroker messageBroker)
        {
            _itemSaleRepository = itemSaleRepository;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(UpdateItemSale command)
        {
            Validate(command);
            var itemSale = await _itemSaleRepository.GetAsync(command.ItemSaleId);

            if (itemSale is null)
            {
                throw new ItemSaleNotFoundException(command.ItemSaleId);
            }

            if (itemSale.Cost == command.ItemCost)
            {
                return;
            }

            itemSale.ChangeCost(command.ItemCost);
            itemSale.ChangeCurrencyCode(command.CurrencyCode);
            await _itemSaleRepository.UpdateAsync(itemSale);
            await _messageBroker.PublishAsync(new IMessage[] { command });
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
