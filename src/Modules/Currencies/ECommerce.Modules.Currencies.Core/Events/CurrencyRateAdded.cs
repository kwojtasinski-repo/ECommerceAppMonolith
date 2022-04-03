using ECommerce.Shared.Abstractions.Events;
using ECommerce.Shared.Abstractions.Time;
using System.Text.Json.Serialization;

namespace ECommerce.Modules.Currencies.Core.Events
{
    internal class CurrencyRateAdded : IEvent
    {
        public CurrencyRateAdded(Guid currencyRateId, decimal rate, string currencyCode, DateOnly rateDate)
        {
            CurrencyRateId = currencyRateId;
            Rate = rate;
            CurrencyCode = currencyCode;
            RateDate = RateDate;
        }

        public Guid CurrencyRateId { get; }
        public decimal Rate { get; }
        public string CurrencyCode { get; }

        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly RateDate { get; }
    }
}
