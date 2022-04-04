using ECommerce.Modules.Sales.Application.Orders.Commands;
using ECommerce.Modules.Sales.Application.Orders.Commands.Handlers;
using ECommerce.Modules.Sales.Domain.Currencies.Entities;
using ECommerce.Modules.Sales.Domain.Currencies.Repositories;
using ECommerce.Modules.Sales.Domain.ItemSales.Entities;
using ECommerce.Modules.Sales.Domain.ItemSales.Repositories;
using ECommerce.Modules.Sales.Domain.Orders.Entities;
using ECommerce.Modules.Sales.Domain.Orders.Repositories;
using ECommerce.Shared.Abstractions.Contexts;
using ECommerce.Shared.Abstractions.Time;
using NSubstitute;
using Xunit;

namespace ECommerce.Modules.Sales.Tests.Unit.Orders.Handlers
{
    public class CreateOrderItemHanderTests
    {
        [Fact]
        public async Task given_valid_command_should_create_order_item()
        {
            var itemSaleId = Guid.NewGuid();
            var currencyCode = "EUR";
            var command = new CreateOrderItem(itemSaleId, currencyCode);
            var itemSale = CreateSampleItemSale(itemSaleId);
            _itemSaleRepository.GetAsync(command.ItemSaleId).Returns(itemSale);
            var currencyCodes = new string[] { currencyCode, itemSale.CurrencyCode };
            var rateDate = DateOnly.FromDateTime(_clock.CurrentDate());
            var rates = new Dictionary<string, decimal>() { { "USD", 2M }, { "EUR", 4M } };
            var currencyRates = CreateSampleCurrencyRates(currencyCodes, rates);
            _currencyRateRepository.GetLatestCurrencyRates(Arg.Any<IEnumerable<string>>()).Returns(currencyRates);

            await _handler.HandleAsync(command);

            await _itemCartRepository.Received(1).AddAsync(Arg.Any<ItemCart>());
            await _orderItemRepository.Received(1).AddAsync(Arg.Any<OrderItem>());
        }

        private ItemSale CreateSampleItemSale(Guid id)
        {
            var item = new Item(Guid.NewGuid(), "Item #1", "Brand #1", "Type #1", "description", null, null);
            var itemSale = new ItemSale(id, item, 100M, "USD");
            return itemSale;
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

        private readonly CreateOrderItemHandler _handler;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IItemSaleRepository _itemSaleRepository;
        private readonly IItemCartRepository _itemCartRepository;
        private readonly IContext _context;
        private readonly ICurrencyRateRepository _currencyRateRepository;
        private readonly IClock _clock;

        public CreateOrderItemHanderTests()
        {
            _orderItemRepository = Substitute.For<IOrderItemRepository>();
            _itemSaleRepository = Substitute.For<IItemSaleRepository>();
            _itemCartRepository = Substitute.For<IItemCartRepository>();
            _context = Substitute.For<IContext>();
            _currencyRateRepository = Substitute.For<ICurrencyRateRepository>();
            _clock = Substitute.For<IClock>();
            _clock.CurrentDate().Returns(DateTime.UtcNow);
            _handler = new CreateOrderItemHandler(_orderItemRepository, _itemSaleRepository,
                                _itemCartRepository, _context, _currencyRateRepository, _clock);
        }
    }
}
