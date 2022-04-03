using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.Sales.Application.Items.Events.External
{
    internal record CurrencyRateUpdated(Guid CurrencyRateId, decimal rate) : IEvent;
}
