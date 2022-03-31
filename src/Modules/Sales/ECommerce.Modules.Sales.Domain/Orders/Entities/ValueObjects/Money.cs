using System.Globalization;

namespace ECommerce.Modules.Sales.Domain.Orders.Entities.ValueObjects
{
    public class Money : IEquatable<Money>
    {
        public static readonly Money Zero = new Money(0);
        private readonly decimal _value;

        public Money(decimal value)
        {
            if (value < 0)
            {
                throw new InvalidOperationException($"Money '{value}' cannot be negative");
            }

            _value = value;
        }

        public static Money operator +(Money money, Money other)
        {
            return new Money(money._value + other._value);
        }
    
        public static Money operator -(Money money, Money other)
        {
            return new Money(money._value - other._value);
        }

        public static Money operator *(Money money, Money other)
        {
            return new Money(money._value * other._value);
        }

        public static Money operator /(Money money, Money other)
        {
            return new Money(money._value / other._value);
        }

        public decimal ToDecimal()
        {
            return _value;
        }

        public bool Equals(Money? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _value == other._value;
        }

        public override bool Equals(object? obj)
        {
            if (obj as Money is null) return false;
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Money)obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<decimal>.Default.GetHashCode(_value);
        }

        public override string ToString()
        {
            var value = (double)_value / 100;
            return value.ToString("0.00", CultureInfo.CreateSpecificCulture("en-US"));
        }
    }
}
