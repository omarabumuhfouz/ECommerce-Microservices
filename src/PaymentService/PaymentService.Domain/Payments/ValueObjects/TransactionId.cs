using PaymentService.Domain.Errors;
using RefundService.Domain.Constants;
using SharedKernel.Shared;

namespace PaymentService.Domain.Payments.ValueObjects;

public sealed record TransactionId
{
    public string Value { get; }

    private TransactionId(string value) => Value = value;

    public static Result<TransactionId> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return DomainErrors.Refund.InvalidTransactionId;

        if (value.Length > RefundConstants.MaxTransactionIdLength) return DomainErrors.Refund.TransactionIdTooLong;

        return new TransactionId(value);
    }
}

