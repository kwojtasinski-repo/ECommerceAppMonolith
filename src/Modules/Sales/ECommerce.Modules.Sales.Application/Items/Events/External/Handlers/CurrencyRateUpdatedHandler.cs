using ECommerce.Modules.Sales.Domain.Currencies.Repositories;
using ECommerce.Shared.Abstractions.Events;
using Microsoft.Extensions.Logging;

namespace ECommerce.Modules.Sales.Application.Items.Events.External.Handlers
{
    internal sealed class CurrencyRateUpdatedHandler : IEventHandler<CurrencyRateUpdated>
    {
        private readonly ICurrencyRateRepository _currencyRateRepository;
        private readonly ILogger<CurrencyRateUpdatedHandler> _logger;

        public CurrencyRateUpdatedHandler(ICurrencyRateRepository currencyRateRepository, ILogger<CurrencyRateUpdatedHandler> logger)
        {
            _currencyRateRepository = currencyRateRepository;
            _logger = logger;
        }

        public async Task HandleAsync(CurrencyRateUpdated @event)
        {
            var currencyRate = await _currencyRateRepository.GetAsync(@event.CurrencyRateId);

            if (currencyRate is null)
            {
                _logger.LogInformation($"CurrencyRate with id: '{@event.CurrencyRateId}' not exists.");
                return;
            }

            currencyRate.Rate = @event.Rate;

            await _currencyRateRepository.UpdateAsync(currencyRate);
            _logger.LogInformation($"Updated an CurrencyRate with id '{currencyRate.Id}'");
        }
    }
}
