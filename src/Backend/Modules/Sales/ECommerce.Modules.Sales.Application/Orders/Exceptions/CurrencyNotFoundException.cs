using ECommerce.Shared.Abstractions.Exceptions;

namespace ECommerce.Modules.Sales.Application.Orders.Exceptions
{
    public class CurrencyNotFoundException : ECommerceException
    {
        public string CurrencyCode { get; }

        public CurrencyNotFoundException(string currencyCode) : base($"Currency code with id '{currencyCode}' not found")
        {
            CurrencyCode = currencyCode;
        }
    }
}
