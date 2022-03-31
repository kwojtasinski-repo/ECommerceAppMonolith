namespace ECommerce.Modules.Sales.Domain.Orders.Entities.ValueObjects
{
    public class Currency : IEquatable<Currency>
    {
        private readonly string _currencyCode;

        public Currency(string currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
            {
                throw new InvalidOperationException("Currency code cannot be null");
            }

            var length = currencyCode.Length;
            if (length != 3)
            {
                throw new InvalidOperationException($"Currency code '{currencyCode}' length '{length}' have to be 3");
            }

            _currencyCode = currencyCode;
        }

        public bool Equals(Currency? other)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<object> GetEqualityComponents()
        {
            yield return _currencyCode;
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
            return $"{_currencyCode}";
        }
    }
}
