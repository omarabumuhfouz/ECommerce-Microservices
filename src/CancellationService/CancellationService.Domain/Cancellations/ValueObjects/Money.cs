using SharedKernel.Shared;
using CancellationService.Domain.Errors;

namespace CancellationService.Domain.ValueObjects;

// 1. Money
public record Money
{
    public decimal Value { get; }

    // Private constructor forces use of Create
    private Money(decimal value) => Value = Math.Round(value, 2);

    public static Money Zero => new(0.00m);

    public static Result<Money> Create(decimal value)
    {
        if (value < 0)
            return DomainErrors.Money.Negative;

        return new Money(value);
    }
    
    // Operators for easy usage
    public static implicit operator decimal(Money money) => money.Value;
}

// 2. Remarks (Renamed)
public record Remarks
{
    public string Value { get; }

    private Remarks(string value) => Value = value;

    public static Result<Remarks> Create(string? value)
    {
        // Null is allowed, but if present, check length
        if (value is not null && value.Length > 500)
            return DomainErrors.Remarks.TooLong;

        return new Remarks(value ?? string.Empty);
    }

    public static Remarks Empty => new(string.Empty);

    public static implicit operator string(Remarks remarks) => remarks.Value;
}


// 3. Reason (Renamed)
public record Reason
{
    public string Value { get; }

    private Reason(string value) => Value = value;

    public static Result<Reason> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return DomainErrors.Reason.Empty;

        if (value.Length > 500)
            return DomainErrors.Reason.TooLong;

        return new Reason(value);
    }

    public static implicit operator string(Reason reason) => reason.Value;
}
