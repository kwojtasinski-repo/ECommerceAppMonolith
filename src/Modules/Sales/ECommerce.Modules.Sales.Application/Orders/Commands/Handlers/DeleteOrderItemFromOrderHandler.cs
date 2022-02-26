using ECommerce.Modules.Sales.Application.Orders.Exceptions;
using ECommerce.Modules.Sales.Application.Orders.Policies;
using ECommerce.Modules.Sales.Domain.Orders.Repositories;
using ECommerce.Shared.Abstractions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Application.Orders.Commands.Handlers
{
    internal class DeleteOrderItemFromOrderHandler : ICommandHandler<DeleteOrderItemFromOrder>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderPositionModificationPolicy _orderPositionModificationPolicy;

        public DeleteOrderItemFromOrderHandler(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IOrderPositionModificationPolicy orderPositionModificationPolicy)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _orderPositionModificationPolicy = orderPositionModificationPolicy;
        }

        public async Task HandleAsync(DeleteOrderItemFromOrder command)
        {
            var orderItem = await _orderItemRepository.GetAsync(command.OrderItemId);
            
            if (orderItem is null)
            {
                throw new OrderItemNotFoundException(command.OrderItemId);
            }

            var order = await _orderRepository.GetDetailsAsync(command.OrderId);

            if (order is null)
            {
                throw new OrderNotFoundException(command.OrderId);
            }

            var canDeletePosition = await _orderPositionModificationPolicy.CanDeleteAsync(order);
            if (!canDeletePosition)
            {
                throw new PositionFromOrderCannotBeDeletedException(order.Id, command.OrderItemId);
            }

            order.DeleteOrderItem(orderItem);
            order.RefreshCost();

            await _orderRepository.UpdateAsync(order);
            await _orderItemRepository.DeleteAsync(orderItem);
        }
    }
}
