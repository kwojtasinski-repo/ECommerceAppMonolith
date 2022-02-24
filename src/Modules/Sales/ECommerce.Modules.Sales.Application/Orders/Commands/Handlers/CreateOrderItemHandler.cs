using ECommerce.Modules.Sales.Application.Orders.Exceptions;
using ECommerce.Modules.Sales.Domain.ItemSales.Repositories;
using ECommerce.Modules.Sales.Domain.Orders.Entities;
using ECommerce.Modules.Sales.Domain.Orders.Repositories;
using ECommerce.Shared.Abstractions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Application.Orders.Commands.Handlers
{
    internal class CreateOrderItemHandler : ICommandHandler<CreateOrderItem>
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IItemSaleRepository _itemSaleRepository;
        private readonly IItemCartRepository _itemCartRepository;

        public CreateOrderItemHandler(IOrderItemRepository orderItemRepository, IItemSaleRepository itemSaleRepository, IItemCartRepository itemCartRepository)
        {
            _orderItemRepository = orderItemRepository;
            _itemSaleRepository = itemSaleRepository;
            _itemCartRepository = itemCartRepository;
        }

        public async Task HandleAsync(CreateOrderItem command)
        {
            var itemSale = await _itemSaleRepository.GetAsync(command.ItemSaleId);

            if (itemSale is null)
            {
                throw new ItemSaleNotFoundException(command.ItemSaleId);
            }

            // snapshot
            var itemCart = new ItemCart(Guid.NewGuid(), itemSale.Item.ItemName, itemSale.Item.BrandName, itemSale.Item.TypeName,
                                        itemSale.Item.Description, itemSale.Item.Tags, itemSale.Item.ImagesUrl, itemSale.Cost);
            await _itemCartRepository.AddAsync(itemCart);

            var orderItem = OrderItem.Create(itemCart, command.UserId);
            await _orderItemRepository.AddAsync(orderItem);
        }
    }
}
