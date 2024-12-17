using ECommerce.Modules.Currencies.Core.Clients.External;
using ECommerce.Modules.Currencies.Core.DTO;
using ECommerce.Shared.Abstractions.Time;
using Microsoft.Extensions.Logging;

namespace ECommerce.Modules.Currencies.Core.Services
{
    internal sealed class CurrencyRateDownloader
        (
            ILogger<CurrencyRateDownloader> logger,
            INbpClient nbpClient,
            ICurrencyService currencyService,
            ICurrencyRateService currencyRateService,
            IClock clock
        )
        : ICurrencyRateDownloader
    {
        public async Task Download(CancellationToken cancellationToken = default)
        {
            var currencyRates = await nbpClient.GetAllCurrenciesForCurrentDay(cancellationToken);
            var date = clock.CurrentDate();

            if (!currencyRates.Any())
            {
                logger.LogError($"There is no CurrencyRates for '{date}'. Please check your address");
                return;
            }

            var rates = currencyRates.SingleOrDefault()?.Rates ?? [];
            if (rates.Count == 0)
            {
                logger.LogError($"There is no Rates for '{date}'. Please check your address");
                return;
            }

            var currencies = await currencyService.GetAllAsync();
            var codes = new List<string>(currencies.Select(c => c.Code));
            var rateDate = DateOnly.FromDateTime(currencyRates.First().EffectiveDate);
            var ratesInDb = await currencyRateService.GetCurrencyRatesForDate(codes, rateDate);
            var ratesFiltered = currencies.Join(rates, c => c.Code.ToUpperInvariant(), r => r.Code.ToUpperInvariant(), (c, r) => r).ToList();
            var currencyPlnExists = currencies.Any(c => c.Code.Equals("PLN", StringComparison.InvariantCultureIgnoreCase));

            if (currencyPlnExists)
            {
                ratesFiltered.Add(new RateTable { Currency = "Polski złoty (PLN)", Code = "PLN", Mid = decimal.One });
            }

            foreach (var currencyRate in ratesFiltered)
            {
                var currency = currencies.FirstOrDefault(c => c.Code == currencyRate.Code);
                if (currency is null)
                {
                    continue;
                }

                var rateInDb = ratesInDb.FirstOrDefault(cr => cr.CurrencyDate == rateDate && cr.CurrencyId == currency.Id);

                if (rateInDb is null)
                {
                    await currencyRateService.AddAsync(new CurrencyRateDto
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
                        await currencyRateService.UpdateAsync(rateInDb);
                    }
                }
            }
        }
    }
}
