using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Modules.Currencies.Tests.Unit.Common
{
    /*public sealed class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var dateString = (string) reader.Value;
            DateTime.TryParse(dateString, out var date);
            return DateOnly.FromDateTime(date);
        }

        public override void WriteJson(JsonWriter writer, DateOnly value, Newtonsoft.Json.JsonSerializer serializer)
        {
            var isoDate = value.ToString("O");
            writer.WriteValue(isoDate);
        }
    }*/
}
