using ECommerce.Modules.Sales.Application.Orders.Commands;
using ECommerce.Modules.Sales.Application.Orders.Commands.Handlers;
using ECommerce.Modules.Sales.Domain.Orders.Entities;
using ECommerce.Modules.Sales.Domain.Orders.Repositories;
using ECommerce.Modules.Sales.Domain.Orders.Services;
using ECommerce.Shared.Abstractions.Contexts;
using ECommerce.Shared.Abstractions.Time;
using NSubstitute;
using Xunit;

namespace ECommerce.Modules.Sales.Tests.Unit.Orders.Handlers
{
    public class CreateOrderHandlerTests
    {
        [Fact]
        public async Task given_valid_command_should_create_order()
        {
            var customerId = Guid.NewGuid();
            var currencyCode = "EUR";
            var command = new CreateOrder(customerId, currencyCode);
            var orderItems = CreateSampleOrderItems();
            _orderItemRepository.GetAllByUserIdNotOrderedAsync(_userId).Returns(orderItems);
            
            await _handler.HandleAsync(command);

            await _orderRepository.Received(1).GetLatestOrderOnDateAsync(_clock.CurrentDate());
            await _orderRepository.Received(1).AddAsync(Arg.Any<Order>());
        }

        private IEnumerable<OrderItem> CreateSampleOrderItems()
        {
            var itemCart1 = new ItemCart(Guid.NewGuid(), "Item #1", "Brand #1", "Type #1",
                                "Description", null, null, 100M, "PLN", _clock.CurrentDate());
            var orderItem1 = new OrderItem(Guid.NewGuid(), itemCart1.Id, itemCart1, 100M, "PLN", decimal.One, _userId);
            
            var itemCart2 = new ItemCart(Guid.NewGuid(), "Item #1", "Brand #1", "Type #1",
                                "Description", null, null, 200M, "USD", _clock.CurrentDate());
            var orderItem2 = new OrderItem(Guid.NewGuid(), itemCart2.Id, itemCart2, 200M, "USD", 2M, _userId);
            
            var itemCart3 = new ItemCart(Guid.NewGuid(), "Item #1", "Brand #1", "Type #1",
                            "Description", null, null, 300M, "EUR", _clock.CurrentDate());
            var orderItem3 = new OrderItem(Guid.NewGuid(), itemCart3.Id, itemCart3, 300M, "EUR", 4M, _userId);

            return new OrderItem[] { orderItem1, orderItem2, orderItem3 };
        }

        private Guid _userId = Guid.NewGuid();
        private readonly CreateOrderHandler _handler;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IClock _clock;
        private readonly IContext _context;
        private readonly IOrderCalculationCostDomainService _orderCalculationCostDomainService;

        public CreateOrderHandlerTests()
        {
            _orderRepository = Substitute.For<IOrderRepository>();
            _orderItemRepository = Substitute.For<IOrderItemRepository>();
            _clock = Substitute.For<IClock>();
            _clock.CurrentDate().Returns(DateTime.UtcNow);
            _context = Substitute.For<IContext>();
            _context.Identity.Id.Returns(_userId);
            _orderCalculationCostDomainService = Substitute.For<IOrderCalculationCostDomainService>();
            _handler = new CreateOrderHandler(_orderRepository, _orderItemRepository,
                                _clock, _context, _orderCalculationCostDomainService);
        }
    }
}
