using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.Sales.Application.Items.Events.External
{
    internal record CurrencyRateAdded(Guid CurrencyRateId, decimal rate, string currencyCode, DateOnly RateDate) : IEvent;
}
