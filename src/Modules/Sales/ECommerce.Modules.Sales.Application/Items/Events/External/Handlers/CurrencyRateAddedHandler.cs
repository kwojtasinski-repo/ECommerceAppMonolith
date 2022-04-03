using ECommerce.Modules.Sales.Domain.Currencies.Entities;
using ECommerce.Modules.Sales.Domain.Currencies.Repositories;
using ECommerce.Shared.Abstractions.Events;
using ECommerce.Shared.Abstractions.Time;
using Microsoft.Extensions.Logging;

namespace ECommerce.Modules.Sales.Application.Items.Events.External.Handlers
{
    internal sealed class CurrencyRateAddedHandler : IEventHandler<CurrencyRateAdded>
    {
        private readonly ICurrencyRateRepository _currencyRateRepository;
        private readonly ILogger<CurrencyRateAddedHandler> _logger;
        private readonly IClock _clock;

        public CurrencyRateAddedHandler(ICurrencyRateRepository currencyRateRepository, ILogger<CurrencyRateAddedHandler> logger,
            IClock clock)
        {
            _currencyRateRepository = currencyRateRepository;
            _logger = logger;
            _clock = clock;
        }

        public async Task HandleAsync(CurrencyRateAdded @event)
        {
            var exists = await _currencyRateRepository.ExistsAsync(@event.CurrencyRateId);

            if (exists)
            {
                _logger.LogInformation($"Currency with id: '{@event.CurrencyRateId}' already exists.");
                return;
            }

            var currencyRate = new CurrencyRate
            {
                Id = @event.CurrencyRateId,
                CurrencyCode = @event.CurrencyCode,
                Rate = @event.Rate,
                RateDate = @event.RateDate,
                Created = DateOnly.FromDateTime(_clock.CurrentDate())
            };

            await _currencyRateRepository.AddAsync(currencyRate);
            _logger.LogInformation($"Added an CurrencyRate with id '{currencyRate.Id}'");
        }
    }
}
