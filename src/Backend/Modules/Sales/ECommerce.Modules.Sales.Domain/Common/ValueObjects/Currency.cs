using System.Globalization;

namespace ECommerce.Modules.Sales.Domain.Orders.Common.ValueObjects
{
    public class Currency : IEquatable<Currency>
    {
        public string CurrencyCode { get; }
        public decimal Rate { get; }

        public Currency(string currencyCode, decimal rate)
        {
            if (string.IsNullOrWhiteSpace(currencyCode) || rate < 0)
            {
                throw new InvalidOperationException("Invalid currency");
            }

            var length = currencyCode.Length;
            if (length != 3)
            {
                throw new InvalidOperationException($"Currency code '{currencyCode}' length '{length}' have to be 3");
            }

            CurrencyCode = currencyCode.ToUpperInvariant();
            Rate = rate;
        }
        
        public Currency ChangeCode(string code)
        {
            return new Currency(code, Rate);
        }

        public Currency ChangeRate(decimal rate)
        {
            return new Currency(CurrencyCode, rate);
        }

        public static Currency Default()
        {
            var currency = new Currency("PLN", decimal.One);
            return currency;
        }

        public bool Equals(Currency? other)
        {
            if (other is null)
            {
                return false;
            }

            return this.GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        private IEnumerable<object> GetEqualityComponents()
        {
            yield return CurrencyCode;
            yield return Rate;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (Currency) obj;

            return this.GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }

        public override string ToString()
        {
            var value = Rate.ToString("0.00", CultureInfo.CreateSpecificCulture("en-US"));
            return $"{value} {CurrencyCode}";
        }
    }
}
