using ECommerce.Modules.Currencies.Core.DTO;

namespace ECommerce.Modules.Currencies.Core.Services
{
    internal interface ICurrencyRateService
    {
        Task<CurrencyRateDto?> GetAsync(Guid id);
        Task<IReadOnlyList<CurrencyRateDto>> GetAllAsync();
        Task<CurrencyRateDto?> GetCurrencyForDateAsync(Guid currencyId, DateOnly date);
        Task<CurrencyRateDto> GetLatestRateAsync(Guid currencyId);
        Task<IReadOnlyList<CurrencyRateDto>> GetCurrencyRatesForDate(IEnumerable<string> codesDistincted, DateOnly date);
        Task AddAsync(CurrencyRateDto currencyRateDto);
        Task UpdateAsync(CurrencyRateDto rateInDb);
        Task<IReadOnlyList<CurrencyRateDto>> GetLatestRatesAsync();
    }
}
