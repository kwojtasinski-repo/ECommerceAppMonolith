using ECommerce.Modules.Sales.Application.Orders.Exceptions;
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

        public DeleteOrderItemFromOrderHandler(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }

        public async Task HandleAsync(DeleteOrderItemFromOrder command)
        {
            var orderItem = await _orderItemRepository.GetAsync(command.OrderItemId);
            
            if (orderItem is null)
            {
                throw new OrderItemNotFoundException(command.OrderItemId);
            }

            var order = await _orderRepository.GetDetailsAsync(command.OrderId);
            order.DeleteOrderItem(orderItem);

            await _orderRepository.UpdateAsync(order);
        }
    }
}
