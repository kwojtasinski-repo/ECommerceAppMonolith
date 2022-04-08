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

        public static string GetCurrencyRateTableJsonString()
        {
            var date = DateOnly.FromDateTime(DateTime.UtcNow);
            var jsonString = new StringBuilder("[{\"table\":\"A\",\"no\":\"069/A/NBP/2022\",\"effectiveDate\":\"");
            jsonString.Append(date.ToString("yyyy-MM-dd"));
            jsonString.Append("\",\"rates\":[{\"currency\":\"bat (Tajlandia)\",\"code\":\"THB\",\"mid\":0.1270},{\"currency\":\"dolar amerykański\",\"code\":\"USD\",\"mid\":4.2703},{\"currency\":\"dolar australijski\",\"code\":\"AUD\",\"mid\":3.1870},{\"currency\":\"dolar Hongkongu\",\"code\":\"HKD\",\"mid\":0.5449},{\"currency\":\"dolar kanadyjski\",\"code\":\"CAD\",\"mid\":3.3913},{\"currency\":\"dolar nowozelandzki\",\"code\":\"NZD\",\"mid\":2.9289},{\"currency\":\"dolar singapurski\",\"code\":\"SGD\",\"mid\":3.1329},{\"currency\":\"euro\",\"code\":\"EUR\",\"mid\":4.6405},{\"currency\":\"forint (Węgry)\",\"code\":\"HUF\",\"mid\":0.012302},{\"currency\":\"frank szwajcarski\",\"code\":\"CHF\",\"mid\":4.5663},{\"currency\":\"funt szterling\",\"code\":\"GBP\",\"mid\":5.5702},{\"currency\":\"hrywna (Ukraina)\",\"code\":\"UAH\",\"mid\":0.1467},{\"currency\":\"jen (Japonia)\",\"code\":\"JPY\",\"mid\":0.034408},{\"currency\":\"korona czeska\",\"code\":\"CZK\",\"mid\":0.1894},{\"currency\":\"korona duńska\",\"code\":\"DKK\",\"mid\":0.6240},{\"currency\":\"korona islandzka\",\"code\":\"ISK\",\"mid\":0.033005},{\"currency\":\"korona norweska\",\"code\":\"NOK\",\"mid\":0.4868},{\"currency\":\"korona szwedzka\",\"code\":\"SEK\",\"mid\":0.4515},{\"currency\":\"kuna (Chorwacja)\",\"code\":\"HRK\",\"mid\":0.6139},{\"currency\":\"lej rumuński\",\"code\":\"RON\",\"mid\":0.9388},{\"currency\":\"lew (Bułgaria)\",\"code\":\"BGN\",\"mid\":2.3727},{\"currency\":\"lira turecka\",\"code\":\"TRY\",\"mid\":0.2895},{\"currency\":\"nowy izraelski szekel\",\"code\":\"ILS\",\"mid\":1.3241},{\"currency\":\"peso chilijskie\",\"code\":\"CLP\",\"mid\":0.005298},{\"currency\":\"peso filipińskie\",\"code\":\"PHP\",\"mid\":0.0828},{\"currency\":\"peso meksykańskie\",\"code\":\"MXN\",\"mid\":0.2120},{\"currency\":\"rand (Republika Południowej Afryki)\",\"code\":\"ZAR\",\"mid\":0.2900},{\"currency\":\"real (Brazylia)\",\"code\":\"BRL\",\"mid\":0.8984},{\"currency\":\"ringgit (Malezja)\",\"code\":\"MYR\",\"mid\":1.0115},{\"currency\":\"rupia indonezyjska\",\"code\":\"IDR\",\"mid\":0.00029734},{\"currency\":\"rupia indyjska\",\"code\":\"INR\",\"mid\":0.056241},{\"currency\":\"won południowokoreański\",\"code\":\"KRW\",\"mid\":0.003482},{\"currency\":\"yuan renminbi (Chiny)\",\"code\":\"CNY\",\"mid\":0.6711},{\"currency\":\"SDR (MFW)\",\"code\":\"XDR\",\"mid\":5.8329}]}]");
            return jsonString.ToString();
        }
    }
}
