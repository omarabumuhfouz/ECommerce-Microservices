using CancellationService.Domain.Errors;

namespace CancellationService.Domain.ValueObjects;

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
