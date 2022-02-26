using ECommerce.Modules.Sales.Application.Orders.Exceptions;
using ECommerce.Modules.Sales.Application.Orders.Policies;
using ECommerce.Modules.Sales.Domain.ItemSales.Repositories;
using ECommerce.Modules.Sales.Domain.Orders.Entities;
using ECommerce.Modules.Sales.Domain.Orders.Repositories;
using ECommerce.Shared.Abstractions.Commands;
using ECommerce.Shared.Abstractions.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Application.Orders.Commands.Handlers
{
    internal class AddOrderItemToOrderHandler : ICommandHandler<AddOrderItemToOrder>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IItemSaleRepository _itemSaleRepository;
        private readonly IItemCartRepository _itemCartRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IContext _context;
        private readonly IOrderPositionModificationPolicy _orderPositionModificationPolicy;

        public AddOrderItemToOrderHandler(IOrderRepository orderRepository, IItemSaleRepository itemSaleRepository, IItemCartRepository itemCartRepository, IOrderItemRepository orderItemRepository, IContext context, IOrderPositionModificationPolicy orderPositionModificationPolicy)
        {
            _orderRepository = orderRepository;
            _itemSaleRepository = itemSaleRepository;
            _itemCartRepository = itemCartRepository;
            _orderItemRepository = orderItemRepository;
            _context = context;
            _orderPositionModificationPolicy = orderPositionModificationPolicy;
        }

        public async Task HandleAsync(AddOrderItemToOrder command)
        {
            var itemSale = await _itemSaleRepository.GetAsync(command.ItemSaleId);

            if (itemSale is null)
            {
                throw new ItemSaleNotFoundException(command.ItemSaleId);
            }

            var order = await _orderRepository.GetDetailsAsync(command.OrderId);

            if (order is null)
            {
                throw new OrderNotFoundException(command.OrderId);
            }

            var canAddPosition = await _orderPositionModificationPolicy.CanAddAsync(order);
            if (!canAddPosition)
            {
                throw new PositionToOrderCannotBeAddedException(order.Id, command.ItemSaleId);
            }

            // snapshot
            var itemCart = new ItemCart(Guid.NewGuid(), itemSale.Item.ItemName, itemSale.Item.BrandName, itemSale.Item.TypeName,
                                        itemSale.Item.Description, itemSale.Item.Tags, itemSale.Item.ImagesUrl, itemSale.Cost);
            await _itemCartRepository.AddAsync(itemCart);
            var orderItem = OrderItem.Create(Guid.NewGuid(), itemCart, _context.Identity.Id);
            await _orderItemRepository.AddAsync(orderItem);

            order.AddOrderItem(orderItem);
            order.RefreshCost();
            await _orderRepository.UpdateAsync(order);
        }
    }
}
