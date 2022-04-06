using ECommerce.Modules.Sales.Application.Orders.Exceptions;
using ECommerce.Modules.Sales.Domain.Orders.Repositories;
using ECommerce.Shared.Abstractions.Commands;

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
            var orderItem = await _orderItemRepository.GetDetailsAsync(command.OrderItemId);

            if (orderItem is null)
            {
                throw new OrderItemNotFoundException(command.OrderItemId);
            }

            if (orderItem.Order is not null)
            {
                throw new OrderItemCannotBeDeletedException(command.OrderItemId);
            }

            await _orderItemRepository.DeleteAsync(orderItem);
        }
    }
}
