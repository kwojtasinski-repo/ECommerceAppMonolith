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
    internal class DeleteOrderItemHandler : ICommandHandler<DeleteOrderItem>
    {
        private readonly IOrderItemRepository _orderItemRepository;

        public DeleteOrderItemHandler(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        public async Task HandleAsync(DeleteOrderItem command)
        {
            var orderItem = await _orderItemRepository.GetAsync(command.OrderItemId);

            if (orderItem is null)
            {
                throw new OrderItemNotFoundException(command.OrderItemId);
            }

            await _orderItemRepository.DeleteAsync(orderItem);
        }
    }
}
