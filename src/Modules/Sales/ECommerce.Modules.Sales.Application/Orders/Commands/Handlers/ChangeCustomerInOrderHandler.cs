using ECommerce.Modules.Sales.Application.Orders.Exceptions;
using ECommerce.Modules.Sales.Domain.Orders.Repositories;
using ECommerce.Shared.Abstractions.Commands;

namespace ECommerce.Modules.Sales.Application.Orders.Commands.Handlers
{
    internal sealed class ChangeCustomerInOrderHandler : ICommandHandler<ChangeCustomerInOrder>
    {
        private readonly IOrderRepository _orderRepository;

        public ChangeCustomerInOrderHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task HandleAsync(ChangeCustomerInOrder command)
        {
            var order = await _orderRepository.GetDetailsAsync(command.OrderId);

            if (order is null)
            {
                throw new OrderNotFoundException(command.OrderId);
            }

            if (order.Paid)
            {
                throw new CustomerCannotBeChangedInOrderException(order.Id);
            }

            order.ChangeCustomer(command.CustomerId);

            await _orderRepository.UpdateAsync(order);
        }
    }
}
