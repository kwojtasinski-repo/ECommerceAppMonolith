using ECommerce.Modules.Sales.Application.Orders.Commands;
using ECommerce.Modules.Sales.Application.Orders.Commands.Handlers;
using ECommerce.Modules.Sales.Application.Orders.Exceptions;
using ECommerce.Modules.Sales.Domain.Currencies.Entities;
using ECommerce.Modules.Sales.Domain.Currencies.Repositories;
using ECommerce.Modules.Sales.Domain.Orders.Entities;
using ECommerce.Modules.Sales.Domain.Orders.Repositories;
using ECommerce.Modules.Sales.Domain.Orders.Services;
using ECommerce.Shared.Abstractions.Time;
using NSubstitute;
using Shouldly;
using Xunit;

namespace ECommerce.Modules.Sales.Tests.Unit.Orders.Handlers
{
    public class ChangeCurrencyInOrderHandlerTests
    {
        [Fact]
        public async Task given_valid_currency_should_change_in_order()
        {
            var order = CreateOrder();
            var orderCost = order.Price.Value;
            _orderRepository.GetDetailsAsync(order.Id).Returns(order);
            var command = new ChangeCurrencyInOrder(order.Id, "EUR");
            var currency = CreateCurrencyRate(command.CurrencyCode, 2);
            var currencyPln = CreateCurrencyRate("PLN", 1);
            _currencyRateRepository.GetLatestCurrencyRate(command.CurrencyCode).Returns(currency);
            _currencyRateRepository.GetLatestCurrencyRates(Arg.Any<IEnumerable<string>>()).Returns(new CurrencyRate[] { currency, currencyPln });

            await _hander.HandleAsync(command);

            await _orderRepository.Received(1).UpdateAsync(order);
            order.Currency.CurrencyCode.ShouldBe(command.CurrencyCode);
            order.Currency.Rate.ShouldBe(currency.Rate);
            order.Price.Value.ShouldBeLessThan(orderCost);
        }

        [Fact]
        public async Task given_invalid_currency_should_throw_an_exception()
        {
            var order = CreateOrder();
            _orderRepository.GetDetailsAsync(order.Id).Returns(order);
            var command = new ChangeCurrencyInOrder(order.Id, "123");
            var expectedException = new CurrencyNotFoundException(command.CurrencyCode);

            var exception = await Record.ExceptionAsync(() => _hander.HandleAsync(command));

            exception.ShouldBeOfType(expectedException.GetType());
            exception.Message.ShouldBe(expectedException.Message);
            ((CurrencyNotFoundException)exception).CurrencyCode.ShouldBe(expectedException.CurrencyCode);
        }
        
        [Fact]
        public async Task given_paid_order_should_throw_an_exception()
        {
            var order = CreateOrder();
            order.MarkAsPaid();
            _orderRepository.GetDetailsAsync(order.Id).Returns(order);
            var command = new ChangeCurrencyInOrder(order.Id, "EUR");
            var expectedException = new CurrencyCannotBeChangedInOrderException(command.OrderId);

            var exception = await Record.ExceptionAsync(() => _hander.HandleAsync(command));

            exception.ShouldBeOfType(expectedException.GetType());
            exception.Message.ShouldBe(expectedException.Message);
            ((CurrencyCannotBeChangedInOrderException)exception).OrderId.ShouldBe(expectedException.OrderId);
        }
                
        [Fact]
        public async Task given_invalid_order_should_throw_an_exception()
        {
            var order = CreateOrder();
            var command = new ChangeCurrencyInOrder(order.Id, "EUR");
            var expectedException = new OrderNotFoundException(command.OrderId);

            var exception = await Record.ExceptionAsync(() => _hander.HandleAsync(command));

            exception.ShouldBeOfType(expectedException.GetType());
            exception.Message.ShouldBe(expectedException.Message);
            ((OrderNotFoundException)exception).OrderId.ShouldBe(expectedException.OrderId);
        }

        private static CurrencyRate CreateCurrencyRate(string currencyCode, decimal rate)
        {
            var date = DateOnly.FromDateTime(DateTime.UtcNow);
            var currencyRate = new CurrencyRate
            {
                Id = Guid.NewGuid(),
                Created = date,
                CurrencyCode = currencyCode,
                Rate = rate,
                RateDate = date
            };

            return currencyRate;
        }

        private static Order CreateOrder()
        {
            var orderItems = new List<OrderItem>
            {
                new OrderItem(Guid.NewGuid(), Guid.NewGuid(),
                            new ItemCart(Guid.NewGuid(), "Name #1", "Brand #1", "Type #1", "This is description", null, null,
                                         100M, "PLN", DateTime.UtcNow), 100M, "PLN", decimal.One, Guid.NewGuid()),
                new OrderItem(Guid.NewGuid(), Guid.NewGuid(),
                            new ItemCart(Guid.NewGuid(), "Name #2", "Brand #2", "Type #2", "This is description", null, null,
                                         200M, "PLN", DateTime.UtcNow), 200M, "PLN", decimal.One, Guid.NewGuid())
            };
            var order = new Order(Guid.NewGuid(), "ORDER", 300M, "PLN", 1M, Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow,
                                    orderItems: orderItems);
            return order;
        }

        private readonly ChangeCurrencyInOrderHandler _hander;
        private readonly IOrderRepository _orderRepository;
        private readonly ICurrencyRateRepository _currencyRateRepository;
        private readonly OrderCalculationCostDomainService _orderCalculationCostDomainService;
        private readonly IClock _clock;

        public ChangeCurrencyInOrderHandlerTests()
        {
            _orderRepository = Substitute.For<IOrderRepository>();
            _currencyRateRepository = Substitute.For<ICurrencyRateRepository>();
            _clock = Substitute.For<IClock>();
            _clock.CurrentDate().Returns(DateTime.UtcNow);
            _orderCalculationCostDomainService = new OrderCalculationCostDomainService(_currencyRateRepository, _clock);
            _hander = new ChangeCurrencyInOrderHandler(_orderRepository, _currencyRateRepository, _orderCalculationCostDomainService);
        }
    }
}
