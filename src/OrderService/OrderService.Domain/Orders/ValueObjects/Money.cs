namespace OrderService.Domain.Orders.ValueObjects;

public record Money : IComparable<Money>
{
    public decimal Value { get; }

    // Private constructor allows operators to bypass validaton 
    // BUT we manually enforce invariants in the operators below.
    private Money() { }
    private Money(decimal value) => Value = decimal.Round(value, 2);

    public static Result<Money> Create(decimal value)
    {
        if (value < 0) return DomainErrors.Money.InvalidAmount;

        return new Money(value);
    }


    // --- 1. Arithmetic Operators ---

    public static Money operator +(Money a, Money b)
    {
        return new Money(a.Value + b.Value);
    }

    public static Money operator -(Money a, Money b)
    {
        if (b.Value > a.Value)
        {
            throw new InvalidOperationException("Money amount cannot be negative.");
        }

        return new Money(a.Value - b.Value);
    }

    public static Money operator *(Money a, int multiplier)
    {
        if (multiplier < 0) throw new ArgumentException("Multiplier cannot be negative");
        return new Money(a.Value * multiplier);
    }

    public static Money operator *(Money a, decimal multiplier)
    {
        if (multiplier < 0) throw new ArgumentException("Multiplier cannot be negative");
        return new Money(a.Value * multiplier);
    }


    public static bool operator <(Money a, Money b) => a.Value < b.Value;
    public static bool operator >(Money a, Money b) => a.Value > b.Value;
    public static bool operator <=(Money a, Money b) => a.Value <= b.Value;
    public static bool operator >=(Money a, Money b) => a.Value >= b.Value;

    public static implicit operator decimal(Money money) => money.Value;

    public static Money Zero => new Money(0);

    public override string ToString() => Value.ToString("C");

    public int CompareTo(Money? other)
    {
        if (other is null) return 1;
        return Value.CompareTo(other.Value);
    }
}
