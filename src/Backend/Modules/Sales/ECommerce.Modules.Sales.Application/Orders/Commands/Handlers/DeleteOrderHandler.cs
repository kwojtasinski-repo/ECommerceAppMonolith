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
    internal class DeleteOrderHandler : ICommandHandler<DeleteOrder>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDeletionPolicy _orderDeletionPolicy;

        public DeleteOrderHandler(IOrderRepository orderRepository, IOrderDeletionPolicy orderDeletionPolicy)
        {
            _orderRepository = orderRepository;
            _orderDeletionPolicy = orderDeletionPolicy;
        }

        public async Task HandleAsync(DeleteOrder command)
        {
            var order = await _orderRepository.GetDetailsAsync(command.OrderId);

            if (order is null)
            {
                throw new OrderNotFoundException(command.OrderId);
            }

            var canDelete = await _orderDeletionPolicy.CanDeleteAsync(order);

            if (canDelete is false)
            {
                throw new OrderCannotBeDeletedException(order.Id);
            }

            await _orderRepository.DeleteAsync(order);
        }
    }
}
