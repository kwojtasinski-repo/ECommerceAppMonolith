using ECommerce.Modules.Currencies.Core.DTO;
using ECommerce.Modules.Currencies.Core.Exceptions;
using ECommerce.Modules.Currencies.Core.Mappings;
using ECommerce.Modules.Currencies.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Core.Services
{
    internal class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyService(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task AddAsync(CurrencyDto dto)
        {
            dto.Id = Guid.NewGuid();
            dto.Code = dto.Code.ToUpper();
            var currency = dto.AsEntity();
            await _currencyRepository.AddAsync(currency);
        }

        public async Task DeleteAsync(Guid id)
        {
            var currency = await _currencyRepository.GetDetailsAsync(id);

            if (currency is null)
            {
                throw new CurrencyNotFoundException(id);
            }

            if (currency.CurrencyRates is not null && currency.CurrencyRates.Any())
            {
                throw new CannotDeleteCurrencyException(id);
            }

            await _currencyRepository.DeleteAsync(currency);
        }

        public async Task<IReadOnlyList<CurrencyDto>> GetAllAsync()
        {
            var currencies = await _currencyRepository.GetAllAsync();
            var dtos = currencies.Select(c => c.AsDto()).ToList();
            return dtos;
        }

        public async Task<CurrencyDetailsDto> GetAsync(Guid id)
        {
            var currency = await _currencyRepository.GetAsync(id);
            var dto = currency?.AsDetailsDto();
            return dto;
        }

        public async Task UpdateAsync(CurrencyDto dto)
        {
            var currency = await _currencyRepository.GetAsync(dto.Id);

            if (currency is null)
            {
                throw new CurrencyNotFoundException(dto.Id);
            }

            currency.Code = dto.Code.ToUpper();
            currency.Description = dto.Description;
            await _currencyRepository.UpdateAsync(currency);
        }
    }
}
