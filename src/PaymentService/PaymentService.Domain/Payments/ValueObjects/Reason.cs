using PaymentService.Domain.Errors;
using RefundService.Domain.Constants;
using SharedKernel.Shared;

namespace PaymentService.Domain.Payments.ValueObjects;

public sealed record Reason
{
    public string Value { get; }

    private Reason(string value) => Value = value;

    public static Result<Reason> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return DomainErrors.Refund.InvalidReason;

        if (value.Length > RefundConstants.MaxReasonLength) return DomainErrors.Refund.ReasonTooLong;

        return new Reason(value);
    }
}
