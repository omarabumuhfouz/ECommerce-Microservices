using CancellationService.Domain.Errors;

namespace CancellationService.Domain.ValueObjects;

public record Money
{
    public decimal Value { get; }

    private Money(decimal value) => Value = Math.Round(value, 2);

    public static Money Zero => new(0.00m);

    public static Result<Money> Create(decimal value)
    {
        if (value < 0)
            return DomainErrors.Money.Negative;

        return new Money(value);
    }
    
    public static implicit operator decimal(Money money) => money.Value;
}
