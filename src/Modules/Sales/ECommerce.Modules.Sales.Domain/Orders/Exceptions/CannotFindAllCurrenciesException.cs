using ECommerce.Shared.Abstractions.Exceptions;

namespace ECommerce.Modules.Sales.Domain.Orders.Exceptions
{
    internal class CannotFindAllCurrenciesException : ECommerceException
    {
        public IEnumerable<string> CurrencyCodes { get; }
        public IEnumerable<string> CurrencyCodesFound { get; }

        public CannotFindAllCurrenciesException(IEnumerable<string> currencyCodes, IEnumerable<string> currencyCodesFound) : base($"Cannot find all currencies '{string.Join(",", currencyCodes)}'. Found '{string.Join(",", currencyCodesFound)}'")
        {
            CurrencyCodes = currencyCodes;
            CurrencyCodesFound = currencyCodesFound;
        }
    }
}
