using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.Currencies.Core.Events
{
    internal record CurrencyRateUpdated(Guid CurrencyRateId, decimal rate) : IEvent;
}
