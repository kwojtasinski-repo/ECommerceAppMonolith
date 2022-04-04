using ECommerce.Modules.Sales.Domain.Currencies.Repositories;
using ECommerce.Modules.Sales.Domain.Orders.Entities;
using ECommerce.Modules.Sales.Domain.Orders.Exceptions;
using ECommerce.Shared.Abstractions.Time;

namespace ECommerce.Modules.Sales.Domain.Orders.Services
{
    public sealed class OrderCalculationCostDomainService : IOrderCalculationCostDomainService
    {
        private readonly ICurrencyRateRepository _currencyRateRepository;
        private readonly IClock _clock;

        public OrderCalculationCostDomainService(ICurrencyRateRepository currencyRateRepository, IClock clock)
        {
            _currencyRateRepository = currencyRateRepository;
            _clock = clock;
        }

        public async Task CalulateOrderCost(Order order)
        {
            var orderItems = order.OrderItems;
            var currencyCodeTarget = order.Currency.CurrencyCode;
            var currencyCodes = orderItems.Select(oi => oi.ItemCart.CurrencyCode).ToList();
            currencyCodes.Add(currencyCodeTarget);
            currencyCodes = currencyCodes.Distinct().ToList(); 
            var date = DateOnly.FromDateTime(_clock.CurrentDate());
            var currencyRates = await _currencyRateRepository.GetLatestCurrencyRates(currencyCodes);

            if (currencyRates.Count() != currencyCodes.Count)
            {
                var currencyCodesFound = currencyRates.Select(cr => cr.CurrencyCode);
                throw new CannotFindAllCurrenciesException(currencyCodes, currencyCodesFound);
            }

            var targetRate = currencyRates.SingleOrDefault(cr => cr.CurrencyCode == currencyCodeTarget);

            foreach (var orderItem in orderItems)
            {
                var currencyCodeSource = orderItem.ItemCart.CurrencyCode;
                var sourceRate = currencyRates.SingleOrDefault(cr => cr.CurrencyCode == currencyCodeSource);
                var sourceCost = orderItem.ItemCart.Price.Value;
                var targetCost = sourceCost * sourceRate.Rate / targetRate.Rate;
                orderItem.ChangeCost(targetCost);
                orderItem.ChangeCurrencyCode(targetRate.CurrencyCode);
                orderItem.ChangeRate(targetRate.Rate);
            }

            order.RefreshCost();
        }
    }
}
