using ECommerce.Modules.Sales.Application.Orders.Commands;
using ECommerce.Modules.Sales.Application.Orders.Commands.Handlers;
using ECommerce.Modules.Sales.Application.Orders.Exceptions;
using ECommerce.Modules.Sales.Application.Orders.Policies;
using ECommerce.Modules.Sales.Domain.ItemSales.Entities;
using ECommerce.Modules.Sales.Domain.ItemSales.Repositories;
using ECommerce.Modules.Sales.Domain.Orders.Entities;
using ECommerce.Modules.Sales.Domain.Orders.Repositories;
using ECommerce.Modules.Sales.Domain.Orders.Services;
using ECommerce.Shared.Abstractions.Contexts;
using ECommerce.Shared.Abstractions.Time;
using NSubstitute;
using Shouldly;
using Xunit;

namespace ECommerce.Modules.Sales.Tests.Unit.Orders.Handlers
{
    public class AddOrderItemToOrderHandlerTests
    {
        [Fact]
        public async Task given_valid_command_should_add_order_item_to_order()
        {
            var orderId = Guid.NewGuid();
            var itemSaleId = Guid.NewGuid();
            var itemSale = CreateSampleItemSale(itemSaleId, Guid.NewGuid(), 100M, "PLN");
            _itemSaleRepository.GetAsync(itemSaleId).Returns(itemSale);
            var order = CreateSampleOrder(orderId, decimal.Zero, "PLN", decimal.One);
            _orderRepository.GetDetailsAsync(orderId).Returns(order);
            _orderPositionModificationPolicy.CanAddAsync(order).Returns(true);
            var command = new AddOrderItemToOrder(orderId, itemSaleId);

            await _handler.HandleAsync(command);

            await _itemCartRepository.Received(1).AddAsync(Arg.Any<ItemCart>());
            await _orderItemRepository.Received(1).AddAsync(Arg.Any<OrderItem>());
            await _orderCalculationCostDomainService.Received(1).CalulateOrderCost(Arg.Any<Order>());
            await _orderRepository.Received(1).UpdateAsync(Arg.Any<Order>());
        }

        [Fact]
        public async Task given_policy_not_allowed_to_modificate_positions_should_throw_an_exception()
        {
            var orderId = Guid.NewGuid();
            var itemSaleId = Guid.NewGuid();
            var itemSale = CreateSampleItemSale(itemSaleId, Guid.NewGuid(), 100M, "PLN");
            _itemSaleRepository.GetAsync(itemSaleId).Returns(itemSale);
            var order = CreateSampleOrder(orderId, decimal.Zero, "PLN", decimal.One);
            _orderRepository.GetDetailsAsync(orderId).Returns(order);
            _orderPositionModificationPolicy.CanAddAsync(order).Returns(false);
            var expectedException = new PositionToOrderCannotBeAddedException(orderId, itemSaleId);
            var command = new AddOrderItemToOrder(orderId, itemSaleId);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(expectedException.GetType());
            ((PositionToOrderCannotBeAddedException)exception).OrderId.ShouldBe(expectedException.OrderId);
            ((PositionToOrderCannotBeAddedException)exception).ItemSaleId.ShouldBe(expectedException.ItemSaleId);
        }

        [Fact]
        public async Task given_invalid_order_id_should_throw_an_exception()
        {
            var orderId = Guid.NewGuid();
            var itemSaleId = Guid.NewGuid();
            var itemSale = CreateSampleItemSale(itemSaleId, Guid.NewGuid(), 100M, "PLN");
            _itemSaleRepository.GetAsync(itemSaleId).Returns(itemSale);
            var expectedException = new OrderNotFoundException(orderId);
            var command = new AddOrderItemToOrder(orderId, itemSaleId);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(expectedException.GetType());
            ((OrderNotFoundException)exception).OrderId.ShouldBe(expectedException.OrderId);
        }

        [Fact]
        public async Task given_invalid_item_sale_id_should_throw_an_exception()
        {
            var orderId = Guid.NewGuid();
            var itemSaleId = Guid.NewGuid();
            var expectedException = new ItemSaleNotFoundException(itemSaleId);
            var command = new AddOrderItemToOrder(orderId, itemSaleId);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType(expectedException.GetType());
            ((ItemSaleNotFoundException)exception).ItemSaleId.ShouldBe(expectedException.ItemSaleId);
        }

        private Order CreateSampleOrder(Guid id, decimal cost, string currencyCode, decimal rate)
        {
            var order = Order.Create(id, "ORDER", cost, currencyCode, rate, Guid.NewGuid(), _userId, _clock.CurrentDate());
            return order;
        }

        private ItemSale CreateSampleItemSale(Guid id, Guid itemId, decimal cost, string currencyCode)
        {
            var item = CreateSampleItem(itemId);
            var itemSale = new ItemSale(id, item, cost, currencyCode);
            return itemSale;
        }

        private Item CreateSampleItem(Guid id)
        {
            var item = new Item(id, "Item #1", "Brand #1", "Type #1", "description", null, null);
            return item;
        }

        private readonly AddOrderItemToOrderHandler _handler;
        private readonly IOrderRepository _orderRepository;
        private readonly IItemSaleRepository _itemSaleRepository;
        private readonly IItemCartRepository _itemCartRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IContext _context;
        private readonly IOrderPositionModificationPolicy _orderPositionModificationPolicy;
        private readonly IOrderCalculationCostDomainService _orderCalculationCostDomainService;
        private readonly IClock _clock;
        private readonly Guid _userId;

        public AddOrderItemToOrderHandlerTests()
        {
            _userId = Guid.NewGuid();
            _orderRepository = Substitute.For<IOrderRepository>();
            _itemSaleRepository = Substitute.For<IItemSaleRepository>();
            _itemCartRepository = Substitute.For<IItemCartRepository>();
            _orderItemRepository = Substitute.For<IOrderItemRepository>();
            _context = Substitute.For<IContext>();
            _context.Identity.Id.Returns(_userId);
            _orderPositionModificationPolicy = Substitute.For<IOrderPositionModificationPolicy>();
            _orderCalculationCostDomainService = Substitute.For<IOrderCalculationCostDomainService>();
            _clock = Substitute.For<IClock>();
            _clock.CurrentDate().Returns(DateTime.UtcNow);
            _handler = new AddOrderItemToOrderHandler(_orderRepository, _itemSaleRepository, _itemCartRepository,
                                    _orderItemRepository, _context, _orderPositionModificationPolicy,
                                    _orderCalculationCostDomainService, _clock);
        }
    }
}
