using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.Currencies.Core.Events
{
    internal record CurrencyRateAdded(Guid CurrencyRateId, decimal rate, string currencyCode, DateOnly RateDate) : IEvent;
}
