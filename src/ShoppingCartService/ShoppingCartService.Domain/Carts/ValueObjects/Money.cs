using SharedKernel.Shared;
using ShoppingCartService.Domain.Errors;

namespace ShoppingCartService.Domain.CartManagement.ValueObjects;

public sealed record class Money
{
    public decimal Value { get; init; }

    private Money(decimal value) => Value = decimal.Round(value, 2);

    public static Result<Money> Create(decimal value)
    {
        if (value < 0) return DomainErrors.Money.InvalidAmount;

        return new Money(value);
    }

    public static Money Zero() => new Money(0);

    public static Money operator +(Money a, Money b) => new Money(a.Value + b.Value);
    public static Money operator -(Money a, Money b) => new Money(a.Value - b.Value);

    public static implicit operator decimal(Money money) => money.Value;
    public static explicit operator Money(decimal value) => new Money(value);
}
