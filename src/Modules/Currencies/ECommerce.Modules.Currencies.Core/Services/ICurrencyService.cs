using ECommerce.Modules.Currencies.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
