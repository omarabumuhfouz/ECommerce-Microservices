using CancellationService.Domain.Errors;

namespace CancellationService.Domain.ValueObjects;

public record Remarks
{
    public string Value { get; }

    private Remarks(string value) => Value = value;

    public static Result<Remarks> Create(string? value)
    {
        if (value is not null && value.Length > 500)
            return DomainErrors.Remarks.TooLong;

        return new Remarks(value ?? string.Empty);
    }

    public static Remarks Empty => new(string.Empty);

    public static implicit operator string(Remarks remarks) => remarks.Value;
}
