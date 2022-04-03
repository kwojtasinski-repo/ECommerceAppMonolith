using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.Currencies.Core.Events
{
    internal record CurrencyRateAdded(Guid CurrencyRateId, decimal Rate, string CurrencyCode, DateOnly RateDate) : IEvent;
}
