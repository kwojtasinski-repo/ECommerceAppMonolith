using ECommerce.Modules.Sales.Application.Orders.Exceptions;
using ECommerce.Modules.Sales.Domain.Orders.Entities;
using ECommerce.Modules.Sales.Domain.Orders.Repositories;
using ECommerce.Shared.Abstractions.Commands;
using ECommerce.Shared.Abstractions.Contexts;
using ECommerce.Shared.Abstractions.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Sales.Application.Orders.Commands.Handlers
{
    internal class CreateOrderHandler : ICommandHandler<CreateOrder>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IClock _clock;
        private readonly IContext _context;

        public CreateOrderHandler(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IClock clock, IContext context)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _clock = clock;
            _context = context;
        }

        public async Task HandleAsync(CreateOrder command)
        {
            var orderItems = await _orderItemRepository.GetAllByUserIdNotOrderedAsync(command.UserId);

            if (!orderItems.Any())
            {
                throw new OrderItemsCannotBeEmptyException();
            }

            var currentDate = _clock.CurrentDate();
            var latestOrder = await _orderRepository.GetLatestOrderOnDateAsync(currentDate);

            int number = 1;
            if (latestOrder is not null)
            {
                var lastOrderNumberToday = latestOrder.OrderNumber;
                var stringNumber = lastOrderNumberToday.Substring(17);//18
                int.TryParse(stringNumber, out number);
                number++;
            }

            decimal cost = decimal.Zero;
            foreach(var orderItem in orderItems)
            {
                cost += orderItem.ItemCart.Cost;
            }

            var orderNumber = new StringBuilder("ORDER/")
                .Append(currentDate.Year).Append('/').Append(currentDate.Month.ToString("d2"))
                .Append('/').Append(currentDate.Day.ToString("00")).Append('/').Append(number).ToString();

            var order = Order.Create(command.Id, orderNumber, cost, command.CustomerId, _context.Identity.Id, currentDate);
            order.AddOrderItems(orderItems);
            await _orderRepository.AddAsync(order);
        }
    }
}
