using ECommerce.Modules.Sales.Domain.Currencies.Entities;
using ECommerce.Modules.Sales.Domain.Currencies.Repositories;
using ECommerce.Modules.Sales.Domain.Orders.Entities;
using ECommerce.Modules.Sales.Domain.Orders.Services;
using ECommerce.Shared.Abstractions.Time;
using NSubstitute;
using Shouldly;
using Xunit;

namespace ECommerce.Modules.Sales.Tests.Unit.Orders.Services
{
    public class OrderCalculationCostDomainServiceTests
    {
        [Fact]
        public async Task given_order_with_valid_currencies_should_calculate_cost()
        {
            var currencyCode = "USD";
            var rate = 2M;
            var rates = new Dictionary<string, decimal> { { "PLN", decimal.One }, { "EUR", 4M }, { "USD", rate } };
            var order = Order.Create(Guid.NewGuid(), "ORD", currencyCode, rate, Guid.NewGuid(), _userId, _clock.CurrentDate());
            var orderItems = CreateSampleOrderItems();
            order.AddOrderItems(orderItems);
            var currencyCodes = orderItems.Select(oi => oi.ItemCart.CurrencyCode).ToList();
            currencyCodes.Add(currencyCode);
            currencyCodes = currencyCodes.Distinct().ToList();
            var date = DateOnly.FromDateTime(_clock.CurrentDate());
            var currencyRates = CreateSampleCurrencyRates(currencyCodes, rates);
            _currencyRateRepository.GetLatestCurrencyRates(Arg.Any<IEnumerable<string>>()).Returns(currencyRates);
            var expectedCost = 850M;

            await _service.CalulateOrderCost(order);

            order.Price.Value.ShouldBeGreaterThan(decimal.One);
            order.Price.Value.ShouldBe(expectedCost);
        }

        private IEnumerable<CurrencyRate> CreateSampleCurrencyRates(IEnumerable<string> currencyCodes, Dictionary<string, decimal> rates)
        {
            var currencyRates = new List<CurrencyRate>();

            foreach (var currencyCode in currencyCodes)
            {
                var random = new Random();
                rates.TryGetValue(currencyCode, out var rate);
                var currencyRate = new CurrencyRate
                {
                    Id = Guid.NewGuid(),
                    CurrencyCode = currencyCode,
                    Rate = rate,
                    RateDate = DateOnly.FromDateTime(_clock.CurrentDate()),
                    Created = DateOnly.FromDateTime(_clock.CurrentDate())
                };
                currencyRates.Add(currencyRate);
            }

            return currencyRates;
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
        private readonly OrderCalculationCostDomainService _service;
        private readonly ICurrencyRateRepository _currencyRateRepository;
        private readonly IClock _clock;

        public OrderCalculationCostDomainServiceTests()
        {
            _currencyRateRepository = Substitute.For<ICurrencyRateRepository>();
            _clock = Substitute.For<IClock>();
            _clock.CurrentDate().Returns(DateTime.UtcNow);
            _service = new OrderCalculationCostDomainService(_currencyRateRepository, _clock);
        }
    }
}
