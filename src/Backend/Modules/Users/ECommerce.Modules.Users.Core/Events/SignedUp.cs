using ECommerce.Shared.Abstractions.Events;

namespace ECommerce.Modules.Users.Core.Events
{
    internal record SignedUp(Guid UserId, string Email) : IEvent;
}
