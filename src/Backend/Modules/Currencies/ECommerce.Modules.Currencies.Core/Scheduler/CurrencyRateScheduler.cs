using ECommerce.Modules.Currencies.Core.Services;
using ECommerce.Shared.Abstractions.SchedulerJobs;
using Microsoft.Extensions.Logging;

namespace ECommerce.Modules.Currencies.Core.Scheduler
{
    internal sealed class CurrencyRateScheduler : ISchedulerTask<CurrencyRateScheduler>
    {
        private readonly ILogger<CurrencyRateScheduler> _logger;
        private readonly ICurrencyRateDownloader _currencyRateDownloader;

        public CurrencyRateScheduler(ILogger<CurrencyRateScheduler> logger, ICurrencyRateDownloader currencyRateDownloader)
        {
            _logger = logger;
            _currencyRateDownloader = currencyRateDownloader;
        }

        public async Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CurrencyRateScheduler started!");
            try
            {
                await _currencyRateDownloader.Download(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CurrencyRateScheduler.DoWork: There was an error");
            }
        }
    }
}
