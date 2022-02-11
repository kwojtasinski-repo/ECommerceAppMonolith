using ECommerce.Modules.Currencies.Core.DTO;
using ECommerce.Modules.Currencies.Core.Repositories;
using ECommerce.Modules.Currencies.Core.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Modules.Currencies.Core.Exceptions;
using ECommerce.Modules.Currencies.Core.Clients;
using ECommerce.Shared.Abstractions.Time;
using ECommerce.Modules.Currencies.Core.Entities;

namespace ECommerce.Modules.Currencies.Core.Services
{
    internal class CurrencyRateService : ICurrencyRateService
    {

        private static readonly DateOnly archiveDate = new DateOnly(2002, 1, 2);
        private static readonly int allowedRequests = 15;

        private readonly ICurrencyRateRepository _currencyRateRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly INbpClient _nbpClient;
        private readonly IClock _clock;

        public CurrencyRateService(ICurrencyRateRepository currencyRateRepository, ICurrencyRepository currencyRepository, INbpClient nbpClient, IClock clock)
        {
            _currencyRateRepository = currencyRateRepository;
            _currencyRepository = currencyRepository;
            _nbpClient = nbpClient;
            _clock = clock;
        }

        public async Task<IReadOnlyList<CurrencyRateDto>> GetAllAsync()
        {
            var currencies = await _currencyRateRepository.GetAllAsync();
            var dtos = currencies.Select(currency => currency.AsDto()).ToList();
            return dtos;
        }

        public async Task<CurrencyRateDto> GetAsync(Guid id)
        {
            var currencyRate = await _currencyRateRepository.GetAsync(id);
            var dto = currencyRate?.AsDto();
            return dto;
        }

        public async Task<CurrencyRateDto> GetCurrencyForDateAsync(Guid currencyId, DateOnly date)
        {
            var currency = await _currencyRepository.GetAsync(currencyId);

            if (currency is null)
            {
                throw new CurrencyNotFoundException(currencyId);
            }

            var rate = await _currencyRateRepository.GetCurrencyRateForDateAsync(currencyId, date);

            var dto = rate?.AsDto();
            return dto;
        }

        public async Task<CurrencyRateDto> GetLatestRateAsync(Guid currencyId)
        {
            var currency = await _currencyRepository.GetAsync(currencyId);

            if (currency is null)
            {
                throw new CurrencyNotFoundException(currencyId);
            }

            var date = _clock.CurrentDate();
            var currencyRate = await GetLatestCurrencyRateAsync(currencyId, currency.Code, DateOnly.FromDateTime(date));

            return currencyRate;
        }

        private async Task<CurrencyRateDto> GetLatestCurrencyRateAsync(Guid currencyId, string currencyCode, DateOnly date) 
        {
            if (currencyCode.ToUpper() == "PLN")
            {
                var currencyRatePln = await TryGetCurrencyRatePln(currencyId, date);
                return currencyRatePln;
            }

            var requestCount = 1;
            var dateStart = date;
            ExchangeRate exchangeRate = null;
            CurrencyRate currencyRate = null;

            while (exchangeRate is null)
            {
                currencyRate = await _currencyRateRepository.GetCurrencyRateForDateAsync(currencyId, dateStart);

                if (currencyRate is not null)
                {
                    return currencyRate.AsDto();
                }

                exchangeRate = await _nbpClient.GetCurrencyRateOnDateAsync(currencyCode, date);

                if (exchangeRate is null)
                {
                    date = date.AddDays(-1);
                }

                if (date < archiveDate)
                {
                    throw new CannotFindCurrencyRateException(currencyId, currencyCode);
                }

                if (requestCount > allowedRequests)
                {
                    throw new CannotFindCurrencyRateException(currencyId, currencyCode);
                }

                requestCount++;
            }

            var rate = exchangeRate.Rates.FirstOrDefault();
           
            if (rate is null)
            {
                throw new RateNotFoundException(currencyCode, date);
            }

            currencyRate = new CurrencyRate
            {
                Id = Guid.NewGuid(),
                CurrencyDate = rate.EffectiveDate,
                CurrencyId = currencyId,
                Rate = rate.Mid
            };

            await _currencyRateRepository.AddAsync(currencyRate);

            return currencyRate.AsDto();
        }

        private async Task<CurrencyRateDto> TryGetCurrencyRatePln(Guid currencyId, DateOnly date)
        {
            var rate = await _currencyRateRepository.GetCurrencyRateForDateAsync(currencyId, date);

            if (rate is not null)
            {
                return rate.AsDto();
            }

            var currencyRate = new CurrencyRate
            {
                Id = Guid.NewGuid(),
                CurrencyDate = date,
                CurrencyId = currencyId,
                Rate = decimal.One
            };

            await _currencyRateRepository.AddAsync(currencyRate);

            return currencyRate.AsDto();
        }
    }
}
