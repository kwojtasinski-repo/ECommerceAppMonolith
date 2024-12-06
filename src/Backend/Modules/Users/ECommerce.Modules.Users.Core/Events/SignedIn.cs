using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.Users.Core.Events
{
    internal record SignedIn(Guid UserId) : IEvent;
}
