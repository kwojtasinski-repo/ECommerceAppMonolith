using ECommerce.Modules.Sales.Application.Orders.Commands;
using ECommerce.Modules.Sales.Application.Orders.Commands.Handlers;
using ECommerce.Modules.Sales.Application.Orders.Policies;
using ECommerce.Modules.Sales.Domain.Orders.Entities;
using ECommerce.Modules.Sales.Domain.Orders.Repositories;
using ECommerce.Modules.Sales.Domain.Orders.Services;
using NSubstitute;
using Xunit;

namespace ECommerce.Modules.Sales.Tests.Unit.Orders.Handlers
{
    public class DeleteOrderItemFromOrderHandlerTests
    {
        [Fact]
        public async Task given_valid_command_should_delete_item_from_order()
        {
            var orderId = Guid.NewGuid();
            var orderItemId = Guid.NewGuid();
            var orderItem = CreateSampleOrderItem(orderItemId);
            var order = CreateSampleOrder(orderId);
            order.AddOrderItem(orderItem);
            var command = new DeleteOrderItemFromOrder(orderId, orderItemId);
            _orderItemRepository.GetAsync(command.OrderItemId).Returns(orderItem);
            _orderRepository.GetDetailsAsync(command.OrderId).Returns(order);
            _orderPositionModificationPolicy.CanDeleteAsync(order).Returns(true);

            await _handler.HandleAsync(command);

            await _orderCalculationCostDomainService.Received(1).CalulateOrderCost(Arg.Any<Order>());
            await _orderRepository.Received(1).UpdateAsync(Arg.Any<Order>());
            await _orderItemRepository.Received(1).DeleteAsync(Arg.Any<OrderItem>());
        }

        private Order CreateSampleOrder(Guid id)
        {
            var order = Order.Create(id, "ORDER", Guid.NewGuid(), Guid.NewGuid(), DateTime.Now);
            return order;
        }

        private OrderItem CreateSampleOrderItem(Guid id)
        {
            var itemCart = CreateSampleItemCart(Guid.NewGuid());
            var orderItem = OrderItem.Create(id, itemCart, 100M, "PLN", 1M, Guid.NewGuid());
            return orderItem;
        }

        private ItemCart CreateSampleItemCart(Guid id)
        {
            var itemCart = new ItemCart(id, "Item #1", "Brand #1", "Type #1", "description",
                                    null, null, 100M, "PLN", DateTime.Now);
            return itemCart;
        }

        private readonly DeleteOrderItemFromOrderHandler _handler;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderPositionModificationPolicy _orderPositionModificationPolicy;
        private readonly IOrderCalculationCostDomainService _orderCalculationCostDomainService;

        public DeleteOrderItemFromOrderHandlerTests()
        {
            _orderRepository = Substitute.For<IOrderRepository>();
            _orderItemRepository = Substitute.For<IOrderItemRepository>();
            _orderPositionModificationPolicy = Substitute.For<IOrderPositionModificationPolicy>();
            _orderCalculationCostDomainService = Substitute.For<IOrderCalculationCostDomainService>();
            _handler = new DeleteOrderItemFromOrderHandler(_orderRepository, _orderItemRepository,
                                _orderPositionModificationPolicy, _orderCalculationCostDomainService);
        }
    }
}
