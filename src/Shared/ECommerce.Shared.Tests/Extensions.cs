using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Tests
{
    public static class Extensions
    {
        public static T GetIdFromHeaders<T>(this IFlurlResponse response, string patternToSplit)
        {
            var type = typeof(T);
            var (responseHeaderName, responseHeaderValue) = response.Headers.Where(h => h.Name == "Location").FirstOrDefault();

            if (responseHeaderValue is null)
            {
                throw new InvalidOperationException("Response header cannot be null");
            }

            var splitedText = responseHeaderValue.Split(patternToSplit + '/');
            var text = splitedText[1];

            if (typeof(Guid).IsAssignableFrom(type))
            {
                var id = ParseToGuid(text);
                return (T)Convert.ChangeType(id, typeof(T));
            }

            throw new InvalidOperationException($"Invalid Type '{type}'");
        }

        private static Guid ParseToGuid(string text)
        {
            var parsed = Guid.TryParse(text, out var id);

            if (!parsed)
            {
                throw new InvalidOperationException($"Cannot parse '{id}' to Guid, please check format");
            }

            return id;
        }
    }
}
