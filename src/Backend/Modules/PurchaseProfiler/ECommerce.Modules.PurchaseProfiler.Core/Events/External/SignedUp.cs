using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.Sales.Application.Items.Events.External
{
    internal record SignedUp(Guid UserId, string Email) : IEvent;
}
