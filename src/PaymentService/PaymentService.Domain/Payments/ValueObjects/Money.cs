using PaymentService.Domain.Errors;
using SharedKernel.Shared;

namespace PaymentService.Domain.Payments.ValueObjects;

public sealed record Money
{
    public decimal Value { get; }

    private Money(decimal value) => Value = value;

    public static Result<Money> Create(decimal value)
    {
        if (value <= 0) return DomainErrors.Refund.InvalidAmount;

        return new Money(decimal.Round(value, 2));
    }
    
    public static implicit operator decimal(Money money) => money.Value;
}



