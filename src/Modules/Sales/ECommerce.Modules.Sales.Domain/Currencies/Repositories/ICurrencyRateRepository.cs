using ECommerce.Modules.Sales.Domain.Currencies.Entities;

namespace ECommerce.Modules.Sales.Domain.Currencies.Repositories
{
    public interface ICurrencyRateRepository
    {
        Task AddAsync(CurrencyRate currencyRate);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> ExistsAsync(string currencyCode, DateOnly createdDate);
        Task<CurrencyRate> GetCurrencyRate(string currencyCode, DateOnly createdDate);
        Task UpdateAsync(CurrencyRate currencyRate);
        Task<IEnumerable<CurrencyRate>> GetCurrencyRatesForDate(IEnumerable<string> currencyCodes, DateOnly date);
        Task<IEnumerable<CurrencyRate>> GetLatestCurrencyRates(IEnumerable<string> currencyCodes);
        Task<CurrencyRate> GetAsync(Guid currencyRateId);
    }
}
