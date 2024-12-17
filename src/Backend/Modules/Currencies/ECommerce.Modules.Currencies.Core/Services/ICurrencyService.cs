using ECommerce.Modules.Currencies.Core.DTO;

namespace ECommerce.Modules.Currencies.Core.Services
{
    internal interface ICurrencyService
    {
        Task AddAsync(CurrencyDto dto);
        Task<CurrencyDetailsDto> GetAsync(Guid id);
        Task<IReadOnlyList<CurrencyDto>> GetAllAsync();
        Task UpdateAsync(CurrencyDto dto);
        Task DeleteAsync(Guid id);
    }
}
