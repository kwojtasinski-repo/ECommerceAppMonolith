using ECommerce.Shared.Abstractions.Exceptions;

namespace ECommerce.Modules.Users.Core.Exceptions
{
    internal class UserNotFoundException : ECommerceException
    {
        public Guid UserId { get; }

        public UserNotFoundException(Guid userId) : base($"User with ID: '{userId}' not found.")
        {
            UserId = userId;
        }
    }
}
