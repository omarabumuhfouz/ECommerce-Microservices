using MediatR;
using PaymentService.Domain.Errors;
using PaymentService.Domain.Payments.Enums;
using PaymentService.Domain.Payments.Events;
using PaymentService.Domain.Payments.ValueObjects;
using SharedKernel.Primitives;
using SharedKernel.Primitives.Result;
using SharedKernel.Shared;

namespace PaymentService.Domain.Payments;

public sealed record Payment : AggregateRoot, IAuditableEntity
{
    private Payment() { }

    private Payment(Guid id, Guid orderId, PaymentMethod paymentMethod, Money amount, string? transactionId)
        : base(id)
    {
        OrderId = orderId;
        Method = paymentMethod;
        Amount = amount;
        TransactionId = transactionId;
        Status = PaymentStatus.Pending;
    }

    public Guid OrderId { get; private set; }
    public PaymentMethod Method { get; private set; } // Now Smart Enum
    public string? TransactionId { get; private set; }
    public Money Amount { get; private set; }
    public PaymentStatus Status { get; private set; } // Now Smart Enum
    private readonly List<Refund> _refunds = new();
    public IReadOnlyCollection<Refund> Refunds => _refunds.AsReadOnly();

    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }

    public static Result<Payment> Create(Guid orderId, PaymentMethod paymentMethod, decimal amount, string? transactionId = null)
    {
        var amountResult = Money.Create(amount);
        if(amountResult.IsFailure) return amountResult.TopError;

         return Result.Success()
            .Ensure(amount > 0, DomainErrors.Payment.InvalidAmount)
            .Map(money => new Payment(Guid.NewGuid(), orderId, paymentMethod, amountResult.Value, transactionId));
    }

    public Result<Unit> UpdateStatus(PaymentStatus newStatus, string? transactionId = null)
    {
        if (Status == newStatus) return Unit.Value;

        return ValidateStateTransition(newStatus)
            .Tap(_ => Status = newStatus)
            .Tap(_ => UpdateTransactionId(transactionId))
            .Tap(_ => RaiseDomainEventForStatusChange());
    }

    private void RaiseDomainEventForStatusChange()
    {
        IDomainEvent? domainEvent = Status.Name switch
        {
            nameof(PaymentStatus.Completed) => Method.IsAutomated
                                            ? new OnlinePaymentCompletedDomainEvent(Id, OrderId, Amount)
                                            : new CodPaymentCompletedDomainEvent(Id, OrderId, Amount),
            nameof(PaymentStatus.Failed) => new PaymentFailedDomainEvent(Id, OrderId, Amount),
            nameof(PaymentStatus.Pending) => new PaymentPendingDomainEvent(Id, OrderId, Amount),
            nameof(PaymentStatus.Refunded) => new PaymentRefundedDomainEvent(Id, OrderId, Amount),
            _ => null
        };

        if (domainEvent is not null) RaiseDomainEvent(domainEvent);
    }

    public Result<Unit> MarkAsCompleted(string? transactionId = null) => UpdateStatus(PaymentStatus.Completed, transactionId);
    public Result<Unit> MarkAsFailed() => UpdateStatus(PaymentStatus.Failed);
    public Result<Unit> MarkAsPending() => UpdateStatus(PaymentStatus.Pending);
    public Result<Unit> MarkAsRefunded() => UpdateStatus(PaymentStatus.Refunded);

    public Result<Unit> UpdateAmount(decimal amount)
    {
        return EnsureIsEditable()
            .Ensure(_ => amount > 0, DomainErrors.Payment.InvalidAmount)
            .Tap(_ => SetAmount(amount))
            .Map(() => Unit.Value);
    }

    private Result SetAmount(decimal amount)
    {
        var newAmount= Money.Create(amount);
        if(newAmount.IsFailure) return newAmount.TopError;
        Amount = newAmount.Value;
        return Result.Success();
    }

    public Result<Unit> UpdateMethod(PaymentMethod method)
        => EnsureIsEditable()
            .Tap(_ => Method = method)
            .Map(_ => Unit.Value);

    public Result<Unit> UpdateDetails(PaymentMethod method, decimal amount)
    {
        return EnsureIsEditable()
            .Bind(_ => UpdateMethod(method))
            .Bind(_ => UpdateAmount(amount));
    }

    private Result<Unit> EnsureIsEditable()
         => Status.IsFinalState
            ? DomainErrors.Payment.CannotUpdate
            : Unit.Value;

    private Result<Unit> ValidateStateTransition(PaymentStatus newStatus)
    {
        if (newStatus == PaymentStatus.Refunded)
        {
            if (Status != PaymentStatus.Completed)
                return DomainErrors.Payment.CannotRefundUncompletedPayment(Status.Name);

            return Unit.Value;
        }

        // If the current status is a final state (Completed/Refunded/Failed), 
        // we generally cannot move to a new status unless it's a specific fix.
        if (Status.IsFinalState && Status != PaymentStatus.Failed)
            return DomainErrors.Payment.InvalidStatusChange(Status.Name, newStatus.Name);

        return Unit.Value;
    }

    public void UpdateTransactionId(string? transactionId)
    {
        if (!string.IsNullOrWhiteSpace(transactionId)) TransactionId = transactionId;
    }

    public bool IsCashOnDelivery() => Method == PaymentMethod.CashOnDelivery;

    public Result<Unit> EnsurePaymentIsCashOnDelivery()
    {
        if (!IsCashOnDelivery()) return DomainErrors.Payment.NotCashOnDelivery(Method.Name);

        return Unit.Value;
    }

    public Result<Unit> RefundPayment(Guid cancellationId, decimal amount, string reason)
    {
        // 1. Logic: Only completed payments can be refunded
        if (Status != PaymentStatus.Completed && Status != PaymentStatus.Refunded)
            return DomainErrors.Payment.CannotRefundUncompletedPayment(Status.Name);

        // 2. Business Rule: Total refunds cannot exceed original amount
        var totalAlreadyRefunded = _refunds
            .Where(r => r.Status != RefundStatus.Failed)
            .Sum(r => r.Amount.Value);

        if (totalAlreadyRefunded + amount > Amount.Value)return DomainErrors.Payment.RefundExceedsOriginalAmount;

        var refundResult = Refund.Create(
            cancellationId,
            this.Id,
            this.OrderId,
            this.Method,
            amount,
            reason);

        if (refundResult.IsFailure) return refundResult.TopError;

        _refunds.Add(refundResult.Value);

        return Unit.Value;
    }

    public Result<Unit> CompleteRefund(Guid refundId,  Guid processedBy, string? transactionId)
    {
        var refund = _refunds.FirstOrDefault(r => r.Id == refundId);

        if (refund is null) return DomainErrors.Refund.NotFound(refundId);

        return refund.SetTransactionId(transactionId)
            .Bind(_ => refund.MarkAsCompleted(DateTime.UtcNow, processedBy))
            // After the refund entity is successful, we update the Aggregate state
            // .Bind(_ =>
            // {
            //     var totalConfirmedAmount = _refunds
            //         .Where(r => r.Status == RefundStatus.Completed)
            //         .Sum(r => r.Amount.Value);

            //     // Update Payment Status based on the total confirmed refunded amount
            //     if (totalConfirmedAmount >= Amount.Value)
            //     {
            //         return  MarkAsRefunded(); // return error
            //     }
            //     // Optional: If you support partially refunded status
            //     else if (totalConfirmedAmount > 0)
            //     {
            //         // Status = PaymentStatus.PartiallyRefunded; 
            //     }

            //     return Result.Success();
            // })
            .Map(_ => Unit.Value);
    }

    public Result<Unit> FailRefund(Guid refundId)
    {
        var refund = _refunds.FirstOrDefault(r => r.Id == refundId);
        if (refund is null) return DomainErrors.Refund.NotFound(refundId);

        refund.MarkAsFailed();

        if (Status == PaymentStatus.Refunded) Status = PaymentStatus.Completed;

        return Unit.Value;
    }

}