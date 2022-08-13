using ECommerce.Shared.Abstractions.Exceptions;

namespace ECommerce.Modules.Items.Domain.Exceptions
{
    internal class InvalidCurrencyException : ECommerceException
    {
        public string CurrencyCode { get; }

        public InvalidCurrencyException(string currencyCode) : base($"Invalid CurrencyCode '{currencyCode}'. CurrencyCode should have 3 characters")
        {
            CurrencyCode = currencyCode;
        }
    }
}
