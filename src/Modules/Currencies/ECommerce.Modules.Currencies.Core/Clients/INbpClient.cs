using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Core.Clients
{
    internal interface INbpClient
    {
        Task<ExchangeRate> GetCurrencyAsync(string currencyCode);
        Task<ExchangeRate> GetCurrencyRateOnDateAsync(string currencyCode, DateOnly dateTime);
    }
}
