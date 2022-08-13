using ECommerce.Shared.Abstractions.Exceptions;

namespace ECommerce.Modules.Currencies.Core.Exceptions
{
    public class CurrencyNotFoundException : ECommerceException
    {
        public Guid CurrencyId { get; }

        public CurrencyNotFoundException(Guid id) : base($"Currency with id: '{id}' was not found.")
        {
            CurrencyId = id;
        }
    }
}
