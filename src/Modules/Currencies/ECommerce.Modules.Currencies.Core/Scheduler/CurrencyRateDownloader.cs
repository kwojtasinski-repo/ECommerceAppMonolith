using ECommerce.Modules.Currencies.Core.Clients.External;
using ECommerce.Modules.Currencies.Core.DTO;
using ECommerce.Modules.Currencies.Core.Services;
using ECommerce.Shared.Abstractions.SchedulerJobs;
using ECommerce.Shared.Abstractions.Time;
using Microsoft.Extensions.Logging;

namespace ECommerce.Modules.Currencies.Core.Scheduler
{
    internal sealed class CurrencyRateDownloader : ISchedulerTask<CurrencyRateDownloader>
    {
        private readonly ILogger<CurrencyRateDownloader> _logger;
        private readonly INbpClient _nbpClient;
        private readonly ICurrencyService _currencyService;
        private readonly ICurrencyRateService _currencyRateService;
        private readonly IClock _clock;

        public CurrencyRateDownloader(ILogger<CurrencyRateDownloader> logger, ICurrencyService currencyService,
            ICurrencyRateService currencyRateService, INbpClient nbpClient, IClock clock)
        {
            _logger = logger;
            _nbpClient = nbpClient;
            _currencyService = currencyService;
            _currencyRateService = currencyRateService;
            _clock = clock;
        }

        public async Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CurrencyRateDownloader started!");
            var currencyRates = await _nbpClient.GetAllCurrenciesForCurrentDay(cancellationToken);
            var date = _clock.CurrentDate();

            if (currencyRates.Count() == 0)
            {
                var message = $"There is no CurrencyRates for '{date}'. Please check your address";
                _logger.LogError(message);
                return;
            }

            var rates = currencyRates.SingleOrDefault().Rates;
            if (rates.Count == 0)
            {
                var message = $"There is no Rates for '{date}'. Please check your address";
                _logger.LogError(message);
                return;
            }

            var currencies = await _currencyService.GetAllAsync();
            var codes = new List<string>(currencies.Select(c => c.Code))
            {
                "PLN"
            };
            var codesDistincted = codes.Distinct();
            var rateDate = DateOnly.FromDateTime(currencyRates.First().EffectiveDate);
            var ratesInDb = await _currencyRateService.GetCurrencyRatesForDate(codesDistincted, rateDate);
            var ratesFiltered = currencies.Join(rates, c => c.Code.ToUpperInvariant(), r => r.Code.ToUpperInvariant(), (c, r) => r).ToList();
            ratesFiltered.Add(new RateTable { Currency = "Polski złoty (PLN)", Code = "PLN", Mid = 1 });

            foreach (var currencyRate in ratesFiltered)
            {
                var currency = currencies.FirstOrDefault(c => c.Code == currencyRate.Code);
                var rateInDb = ratesInDb.SingleOrDefault(cr => cr.CurrencyDate == rateDate && cr.CurrencyId == currency.Id);
                
                if (rateInDb is null)
                {
                    await _currencyRateService.AddAsync(new CurrencyRateDto
                    {
                        CurrencyId = currency.Id,
                        CurrencyDate = rateDate,
                        Rate = currencyRate.Mid
                    });
                }
                else
                {
                    if (rateInDb.Rate != currencyRate.Mid)
                    {
                        rateInDb.Rate = currencyRate.Mid;
                        await _currencyRateService.UpdateAsync(rateInDb);
                    }
                }
            }
        }
    }
}
