using System.Globalization;

namespace ECommerce.Modules.Sales.Domain.Orders.Common.ValueObjects
{
    public class Money : IEquatable<Money>
    {
        public static readonly Money Zero = new Money(0);
        public decimal Value { get; }

        public Money(decimal value)
        {
            if (value < 0)
            {
                throw new InvalidOperationException($"Money '{value}' cannot be negative");
            }

            Value = value;
        }

        public static Money operator +(Money money, Money other)
        {
            return new Money(money.Value + other.Value);
        }
    
        public static Money operator -(Money money, Money other)
        {
            return new Money(money.Value - other.Value);
        }

        public static Money operator *(Money money, Money other)
        {
            return new Money(money.Value * other.Value);
        }

        public static Money operator /(Money money, Money other)
        {
            return new Money(money.Value / other.Value);
        }

        public Money ChangeValue(decimal value)
        {
            return new Money(value);
        }

        public bool Equals(Money? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
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
            return EqualityComparer<decimal>.Default.GetHashCode(Value);
        }

        public override string ToString()
        {
            return Value.ToString("0.00", CultureInfo.CreateSpecificCulture("en-US"));
        }
    }
}
