using ECommerce.Modules.Currencies.Core.DTO;
using ECommerce.Modules.Currencies.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Api.Controllers
{
    internal class CurrencyRatesController : BaseController
    {
        private readonly ICurrencyRateService _currencyRateService;

        public CurrencyRatesController(ICurrencyRateService currencyRateService)
        {
            _currencyRateService = currencyRateService;
        }

        [HttpGet("{currencyId:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CurrencyRateDto>> GetAsync(Guid currencyId)
        {
            var currency = await _currencyRateService.GetLatestRateAsync(currencyId);
            return OkOrNotFound(currency);
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CurrencyRateDto>> GetCurrencyRateForDateAsync([FromQuery] Guid currencyId, [FromQuery] DateOnly date)
        {
            var currency = await _currencyRateService.GetCurrencyForDateAsync(currencyId, date);
            return OkOrNotFound(currency);
        }
    }
}
