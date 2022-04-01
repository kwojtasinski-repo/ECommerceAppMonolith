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
            var currency = await _currencyRateRepository.GetCurrencyRate(itemSale.CurrencyCode, rateDate);
            if (currency is null)
            {
                throw new CurrencyNotFoundException(itemSale.CurrencyCode, rateDate);
            }

            // snapshot
            var itemCart = new ItemCart(Guid.NewGuid(), itemSale.Item.ItemName, itemSale.Item.BrandName, itemSale.Item.TypeName,
                                        itemSale.Item.Description, itemSale.Item.Tags, itemSale.Item.ImagesUrl, itemSale.Cost, itemSale.CurrencyCode);
            await _itemCartRepository.AddAsync(itemCart);

            var currencyDefault = Currency.Default(); // domyslna waluta PLN
            var cost = itemCart.Price.Value * currency.Rate;
            var orderItem = OrderItem.Create(command.OrderItemId, itemCart, cost, currencyDefault.CurrencyCode, currency.Rate, _context.Identity.Id);
            await _orderItemRepository.AddAsync(orderItem);
        }
    }
}
