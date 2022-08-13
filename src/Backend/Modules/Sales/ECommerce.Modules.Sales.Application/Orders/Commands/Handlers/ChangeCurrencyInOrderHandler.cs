using ECommerce.Modules.Sales.Application.Orders.Exceptions;
using ECommerce.Modules.Sales.Domain.Currencies.Repositories;
using ECommerce.Modules.Sales.Domain.Orders.Repositories;
using ECommerce.Modules.Sales.Domain.Orders.Services;
using ECommerce.Shared.Abstractions.Commands;

namespace ECommerce.Modules.Sales.Application.Orders.Commands.Handlers
{
    internal class ChangeCurrencyInOrderHandler : ICommandHandler<ChangeCurrencyInOrder>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICurrencyRateRepository _currencyRateRepository;
        private readonly IOrderCalculationCostDomainService _orderCalculationCostDomainService;

        public ChangeCurrencyInOrderHandler(IOrderRepository orderRepository, ICurrencyRateRepository currencyRateRepository, 
                IOrderCalculationCostDomainService orderCalculationCostDomainService)
        {
            _orderRepository = orderRepository;
            _currencyRateRepository = currencyRateRepository;
            _orderCalculationCostDomainService = orderCalculationCostDomainService;
        }

        public async Task HandleAsync(ChangeCurrencyInOrder command)
        {
            var order = await _orderRepository.GetDetailsAsync(command.OrderId);

            if (order is null)
            {
                throw new OrderNotFoundException(command.OrderId);
            }

            if (order.Paid)
            {
                throw new CurrencyCannotBeChangedInOrderException(order.Id);
            }

            var currency = await _currencyRateRepository.GetLatestCurrencyRate(command.CurrencyCode);

            if (currency is null)
            {
                throw new CurrencyNotFoundException(command.CurrencyCode);
            }

            order.ChangeCurrency(currency.CurrencyCode, currency.Rate);
            await _orderCalculationCostDomainService.CalulateOrderCost(order);
            await _orderRepository.UpdateAsync(order);
        }
    }
}
