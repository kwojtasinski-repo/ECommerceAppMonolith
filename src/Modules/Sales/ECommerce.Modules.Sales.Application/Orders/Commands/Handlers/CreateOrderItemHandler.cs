using ECommerce.Modules.Sales.Application.Orders.Exceptions;
using ECommerce.Modules.Sales.Domain.Currencies.Repositories;
using ECommerce.Modules.Sales.Domain.ItemSales.Repositories;
using ECommerce.Modules.Sales.Domain.Orders.Common.ValueObjects;
using ECommerce.Modules.Sales.Domain.Orders.Entities;
using ECommerce.Modules.Sales.Domain.Orders.Repositories;
using ECommerce.Shared.Abstractions.Commands;
using ECommerce.Shared.Abstractions.Contexts;
using ECommerce.Shared.Abstractions.Time;

namespace ECommerce.Modules.Sales.Application.Orders.Commands.Handlers
{
    internal class CreateOrderItemHandler : ICommandHandler<CreateOrderItem>
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IItemSaleRepository _itemSaleRepository;
        private readonly IItemCartRepository _itemCartRepository;
        private readonly IContext _context; 
        private readonly ICurrencyRateRepository _currencyRateRepository;
        private readonly IClock _clock;

        public CreateOrderItemHandler(IOrderItemRepository orderItemRepository, IItemSaleRepository itemSaleRepository, IItemCartRepository itemCartRepository, IContext context, ICurrencyRateRepository currencyRateRepository, IClock clock)
        {
            _orderItemRepository = orderItemRepository;
            _itemSaleRepository = itemSaleRepository;
            _itemCartRepository = itemCartRepository;
            _context = context;
            _currencyRateRepository = currencyRateRepository;
            _clock = clock;
        }

        public async Task HandleAsync(CreateOrderItem command)
        {
            var itemSale = await _itemSaleRepository.GetAsync(command.ItemSaleId);

            if (itemSale is null)
            {
                throw new ItemSaleNotFoundException(command.ItemSaleId);
            }

            var rateDate = DateOnly.FromDateTime(_clock.CurrentDate());
            IEnumerable<string> currencyCodes = new string[] { command.CurrencyCode, itemSale.CurrencyCode };
            currencyCodes = currencyCodes.Distinct();
            var currencyRates = await _currencyRateRepository.GetCurrencyRatesForDate(currencyCodes, rateDate);
            
            if (currencyRates.Count() != currencyCodes.Count())
            {
                var currencyCodesFound = currencyRates.Select(cr => cr.CurrencyCode);
                throw new InvalidCurrenciesException(currencyCodes, currencyCodesFound);
            }

            // snapshot
            var itemCart = new ItemCart(Guid.NewGuid(), itemSale.Item.ItemName, itemSale.Item.BrandName, itemSale.Item.TypeName,
                                        itemSale.Item.Description, itemSale.Item.Tags, itemSale.Item.ImagesUrl, itemSale.Cost, itemSale.CurrencyCode,
                                        _clock.CurrentDate());
            await _itemCartRepository.AddAsync(itemCart);

            var currencyCodeTarget = command.CurrencyCode;
            var currencyCodeSource = itemCart.CurrencyCode;
            var targetRate = currencyRates.SingleOrDefault(cr => cr.CurrencyCode == currencyCodeTarget);
            var sourceRate = currencyRates.SingleOrDefault(cr => cr.CurrencyCode == currencyCodeSource);
            var sourceCost = itemCart.Price.Value;
            var cost = sourceCost * sourceRate.Rate / targetRate.Rate;

            var orderItem = OrderItem.Create(command.OrderItemId, itemCart, cost, currencyCodeTarget, targetRate.Rate, _context.Identity.Id);
            await _orderItemRepository.AddAsync(orderItem);
        }
    }
}
