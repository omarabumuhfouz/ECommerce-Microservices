using Ardalis.SmartEnum;

namespace PaymentService.Domain.Payments.Enums;

public abstract class PaymentStatus : SmartEnum<PaymentStatus>
{
    public static readonly PaymentStatus Pending = new PendingStatus();
    public static readonly PaymentStatus Completed = new CompletedStatus();
    public static readonly PaymentStatus Refunded = new RefundedStatus();
    public static readonly PaymentStatus Failed = new FailedStatus();

    public bool IsFinalState { get; }

    private PaymentStatus(string name, int value, bool isFinalState) : base(name, value)
    {
        IsFinalState = isFinalState;
    }

    private sealed class PendingStatus : PaymentStatus { public PendingStatus() : base("Pending", 1, false) { } }
    private sealed class CompletedStatus : PaymentStatus { public CompletedStatus() : base("Completed", 2, true) { } }
    private sealed class RefundedStatus : PaymentStatus { public RefundedStatus() : base("Refunded", 3, true) { } }
    private sealed class FailedStatus : PaymentStatus { public FailedStatus() : base("Failed", 4, true) { } }
}