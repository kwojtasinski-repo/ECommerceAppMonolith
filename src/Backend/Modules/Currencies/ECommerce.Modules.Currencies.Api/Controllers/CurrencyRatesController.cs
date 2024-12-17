using ECommerce.Modules.Currencies.Core.DTO;
using ECommerce.Modules.Currencies.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Modules.Currencies.Api.Controllers
{
    internal class CurrencyRatesController : BaseController
    {
        private readonly ICurrencyRateService _currencyRateService;
        private readonly ICurrencyRateDownloader _currencyRateDownloader;

        public CurrencyRatesController(ICurrencyRateService currencyRateService, ICurrencyRateDownloader currencyRateDownloader)
        {
            _currencyRateService = currencyRateService;
            _currencyRateDownloader = currencyRateDownloader;
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

        [HttpGet("latest")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IReadOnlyList<CurrencyRateDto>>> GetLatestCurrencies()
        {
            var currencyRates = await _currencyRateService.GetLatestRatesAsync();
            return Ok(currencyRates);
        }

        [Authorize("currencies")]
        [HttpGet("refresh")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> RefreshCurrencies()
        {
            await _currencyRateDownloader.Download();
            return Ok();
        }
    }
}
