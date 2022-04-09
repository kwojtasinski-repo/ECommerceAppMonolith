using ECommerce.Shared.Abstractions.Time;
using System.Text.Json.Serialization;

namespace ECommerce.Modules.Currencies.Core.DTO
{
    internal class CurrencyRateDto
    {
        public Guid Id { get; set; }
        public Guid CurrencyId { get; set; }
        public decimal Rate { get; set; }
        public string Code { get; set; }
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly CurrencyDate { get; set; }
    }
}
