using ECommerce.Shared.Abstractions.SchedulerJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Core.Scheduler
{
    internal sealed class CurrencyRateDownloader : ISchedulerTask<CurrencyRateDownloader>
    {
        private ILogger<CurrencyRateDownloader> _logger;

        public CurrencyRateDownloader(ILogger<CurrencyRateDownloader> logger)
        {
            _logger = logger;
        }

        public async Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CurrencyRateDownloader started!");
        }
    }
}
