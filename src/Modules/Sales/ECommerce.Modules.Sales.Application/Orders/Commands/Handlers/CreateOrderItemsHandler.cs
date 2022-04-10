using ECommerce.Modules.Sales.Application.Orders.Exceptions;
using ECommerce.Modules.Sales.Domain.Currencies.Repositories;
using ECommerce.Modules.Sales.Domain.ItemSales.Repositories;
using ECommerce.Modules.Sales.Domain.Orders.Entities;
using ECommerce.Modules.Sales.Domain.Orders.Repositories;
using ECommerce.Shared.Abstractions.Commands;
using ECommerce.Shared.Abstractions.Contexts;
using ECommerce.Shared.Abstractions.Time;

namespace ECommerce.Modules.Sales.Application.Orders.Commands.Handlers
{
    internal class CreateOrderItemsHandler : ICommandHandler<CreateOrderItems>
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IItemSaleRepository _itemSaleRepository;
        private readonly IItemCartRepository _itemCartRepository;
        private readonly IContext _context;
        private readonly ICurrencyRateRepository _currencyRateRepository;
        private readonly IClock _clock;

        public CreateOrderItemsHandler(IOrderItemRepository orderItemRepository, IItemSaleRepository itemSaleRepository,
                    IItemCartRepository itemCartRepository, IContext context, ICurrencyRateRepository currencyRateRepository,
                    IClock clock)
        {
            _orderItemRepository = orderItemRepository;
            _itemSaleRepository = itemSaleRepository;
            _itemCartRepository = itemCartRepository;
            _context = context;
            _currencyRateRepository = currencyRateRepository;
            _clock = clock;
        }

        public async Task HandleAsync(CreateOrderItems command)
        {
            var itemSalesIds = command.ItemSaleIds;

            foreach (var itemSaleId in itemSalesIds)
            {
                var itemSale = await _itemSaleRepository.GetAsync(itemSaleId);

                if (itemSale is null)
                {
                    throw new ItemSaleNotFoundException(itemSaleId);
                }

                IEnumerable<string> currencyCodes = new string[] { command.CurrencyCode, itemSale.CurrencyCode };
                currencyCodes = currencyCodes.Distinct();
                var currencyRates = await _currencyRateRepository.GetLatestCurrencyRates(currencyCodes);

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

                var orderItemId = Guid.NewGuid();
                var orderItem = OrderItem.Create(orderItemId, itemCart, itemCart.Price.Value, sourceRate.CurrencyCode, sourceRate.Rate, targetRate.CurrencyCode, targetRate.Rate, _context.Identity.Id);
                await _orderItemRepository.AddAsync(orderItem);
            }
        }
    }
}
