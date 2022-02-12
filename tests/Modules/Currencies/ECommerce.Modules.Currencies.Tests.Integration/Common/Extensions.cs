using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Tests.Integration.Common
{
    internal static class Extensions
    {
        public static string GetSampleCurrencyRateJsonString(DateOnly date = new DateOnly(), string code = "eur")
        {
            var defaultDate = date == new DateOnly() ? new DateOnly(2022, 2, 11) : date;
            var defaultDateString = defaultDate.ToString("yyyy-MM-dd");
            return "{\"table\":\"A\",\"currency\":\"euro\",\"code\":\"" + code + "\",\"rates\":[{\"no\":\"029/A/NBP/2022\",\"effectiveDate\":" + $"\"{defaultDateString}" + "\",\"mid\":4.5163}]}";
        }        
    }
}
