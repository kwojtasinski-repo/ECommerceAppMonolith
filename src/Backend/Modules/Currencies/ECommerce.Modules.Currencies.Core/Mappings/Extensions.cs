﻿using ECommerce.Modules.Currencies.Core.DTO;
using ECommerce.Modules.Currencies.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Core.Mappings
{
    internal static class Extensions
    {
        public static Currency AsEntity(this CurrencyDto currencyDto)
        {
            var currency = new Currency
            {
                Id = currencyDto.Id,
                Code = currencyDto.Code,
                Description = currencyDto.Description
            };

            return currency;
        }

        public static CurrencyDto AsDto(this Currency currency)
        {
            var currencyDto = new CurrencyDto
            {
                Id = currency.Id,
                Code = currency.Code,
                Description = currency.Description
            };

            return currencyDto;
        }

        public static CurrencyDetailsDto AsDetailsDto(this Currency currency)
        {
            var currencyDto = new CurrencyDetailsDto
            {
                Id = currency.Id,
                Code = currency.Code,
                Description = currency.Description,
                CurrencyRates = currency.CurrencyRates?.Select(cr => cr.AsDto()).ToList()
            };

            return currencyDto;
        }

        public static Currency AsEntity(this CurrencyDetailsDto currencyDto)
        {
            var currency = new Currency
            {
                Id = currencyDto.Id,
                Code = currencyDto.Code,
                Description = currencyDto.Description,
                CurrencyRates = currencyDto.CurrencyRates
                .Select(cr => new CurrencyRate 
                              { 
                                    Id = cr.Id, 
                                    CurrencyId = cr.CurrencyId, 
                                    CurrencyDate = cr.CurrencyDate, 
                                    Rate = cr.Rate 
                              }
                        ).ToList()
            };

            return currency;
        }

        public static CurrencyRateDto AsDto(this CurrencyRate currencyRate)
        {
            var dto = new CurrencyRateDto
            {
                Id = currencyRate.Id,
                CurrencyId = currencyRate.CurrencyId,
                CurrencyDate = currencyRate.CurrencyDate,
                Rate = currencyRate.Rate,
                Code = currencyRate.Currency?.Code
            };

            return dto;
        }

        public static CurrencyRate AsEntity(this CurrencyRateDto dto)
        {
            var currencyRate = new CurrencyRate
            {
                Id = dto.Id,
                CurrencyId = dto.CurrencyId,
                CurrencyDate = dto.CurrencyDate,
                Rate = dto.Rate
            };

            return currencyRate;
        }
    }
}
