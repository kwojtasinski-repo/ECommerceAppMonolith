using System.Globalization;

namespace ECommerce.Modules.Sales.Domain.Orders.Entities.ValueObjects
{
    public class Currency : IEquatable<Currency>
    {
        private readonly string _currencyCode;
        private readonly decimal _rate;

        public Currency(string currencyCode, decimal rate)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
            {
                throw new InvalidOperationException("Currency code cannot be null");
            }

            if (rate < 0)
            {
                throw new InvalidOperationException($"Rate '{rate}' cannot be negative");
            }

            _currencyCode = currencyCode;
            _rate = rate;
        }

        public bool Equals(Currency? other)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<object> GetEqualityComponents()
        {
            yield return _currencyCode;
            yield return _rate;
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
            var value = _rate / 100;
            value.ToString("0.00", CultureInfo.CreateSpecificCulture("en-US"));
            return $"{value} {_currencyCode}";
        }
    }
}
