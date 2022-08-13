using ECommerce.Shared.Abstractions.Exceptions;

namespace ECommerce.Modules.Contacts.Core.Exceptions.Customers
{
    internal class PhoneNumberCannotBeNullException : ECommerceException
    {
        public PhoneNumberCannotBeNullException() : base("PhoneNumber cannot be null.")
        {
        }
    }
}