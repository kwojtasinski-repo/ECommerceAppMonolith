using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Core.Clients
{
    internal interface INbpClient
    {
        Task<string> GetCurrency(string currencyCode);
        Task<string> GetCurrencyRateOnDate(string currencyCode, DateTime dateTime);
    }
}
