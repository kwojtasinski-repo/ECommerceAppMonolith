using ECommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Users.Core.Exceptions
{
    internal class UserNotActiveException : ECommerceException
    {
        public Guid UserId { get; }

        public UserNotActiveException(Guid userId) : base($"User with ID: '{userId}' is not active.")
        {
            UserId = userId;
        }
    }
}
