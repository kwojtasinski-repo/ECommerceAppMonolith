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
    internal class DeleteOrderItemHandler : ICommandHandler<DeleteOrder>
    {
        private readonly IOrderItemRepository _orderItemRepository;

        public DeleteOrderItemHandler(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        public async Task HandleAsync(DeleteOrder command)
        {
            var orderItem = await _orderItemRepository.GetAsync(command.OrderId);

            if (orderItem is null)
            {
                throw new OrderItemNotFoundException(command.OrderId);
            }

            await _orderItemRepository.DeleteAsync(orderItem);
        }
    }
}
