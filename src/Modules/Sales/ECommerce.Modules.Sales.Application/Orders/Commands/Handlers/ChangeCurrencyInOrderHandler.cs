using ECommerce.Modules.Sales.Application.Orders.Exceptions;
using ECommerce.Modules.Sales.Domain.Currencies.Repositories;
using ECommerce.Modules.Sales.Domain.Orders.Repositories;
using ECommerce.Shared.Abstractions.Commands;

namespace ECommerce.Modules.Sales.Application.Orders.Commands.Handlers
{
    internal class ChangeCurrencyInOrderHandler : ICommandHandler<ChangeCurrencyInOrder>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICurrencyRateRepository _currencyRateRepository;

        public ChangeCurrencyInOrderHandler(IOrderRepository orderRepository, ICurrencyRateRepository currencyRateRepository)
        {
            _orderRepository = orderRepository;
            _currencyRateRepository = currencyRateRepository;
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
                throw new CustomerCannotBeChangedInOrderException(order.Id);
            }

            var currency = await _currencyRateRepository.GetLatestCurrencyRate(command.CurrencyCode);
           
            if(currency is null)
            {
                throw new CurrencyNotFoundException(command.CurrencyCode);
            }

            order.ChangeCurrency(currency.CurrencyCode, currency.Rate);
            await _orderRepository.UpdateAsync(order);
        }
    }
}
