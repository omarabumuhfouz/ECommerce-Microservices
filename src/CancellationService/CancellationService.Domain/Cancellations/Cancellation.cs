using CancellationService.Domain.Cancellations.Enums;
using CancellationService.Domain.ValueObjects;
using CancellationService.Domain.Errors;
using CancellationService.Domain.Cancellations.Events;
using MediatR;
using SharedKernel.Primitives.Results;

namespace CancellationService.Domain.Cancellations;

public class Cancellation : AggregateRoot
{
    private Cancellation()
    {
        Reason = Reason.Create("None").Value;
        OrderAmount = Money.Zero;
    }

    private Cancellation(
        Guid id,
        Guid orderId,
        Reason reason,
        Money orderAmount) : base(id)
    {
        OrderId = orderId;
        Reason = reason;
        OrderAmount = orderAmount;
        Status = CancellationStatus.Pending;
        RequestedAt = DateTime.UtcNow;
        ProcessedAt = null;
        ProcessedBy = null;
    }



    public Guid OrderId { get; private set; }
    public Reason Reason { get; private set; }
    public CancellationStatus Status { get; private set; }
    public DateTime RequestedAt { get; private set; }
    public Money OrderAmount { get; private set; }
    public Money Charges { get; private set; } = Money.Zero;
    public Remarks Remarks { get; private set; } = Remarks.Empty;

    public DateTime? ProcessedAt { get; private set; }
    public Guid? ProcessedBy { get; private set; }


    public static Result<Cancellation> Create(
        Guid orderId,
        string reasonString,
        decimal orderAmount)
    {
        var reasonResult = Reason.Create(reasonString);
        var orderAmountResult = Money.Create(orderAmount);

        if (reasonResult.IsFailure) return reasonResult.TopError;
        if (orderAmountResult.IsFailure) return orderAmountResult.TopError;

        var cancellation = new Cancellation(
            Guid.NewGuid(),
            orderId,
            reasonResult.Value,
            orderAmountResult.Value
        );

        return cancellation;
    }


    public Result<Unit> Reject(string remarks, Guid rejectedBy)
    {
        return Result.Success()
            .Ensure(_ => Status == CancellationStatus.Pending, DomainErrors.Cancellation.AlreadyProcessed)
            .Bind(_ => ApplyProcessingDetails(remarks, rejectedBy, CancellationStatus.Rejected))
            .Tap(_ => RaiseDomainEvent(new CancellationRejectedDomainEvent(Id, OrderId, remarks)))
            .Map(_ => Unit.Value);
    }

    public Result<Unit> Approve(string remarks, decimal charges, Guid approvedBy)
    {
        return Result.Success()
            .Ensure(_ => Status == CancellationStatus.Pending, DomainErrors.Cancellation.AlreadyProcessed)
            .Bind(_ => SetChaarges(charges))
            .Bind(_ => ApplyProcessingDetails(remarks, approvedBy, CancellationStatus.Approved))
            .Tap(_ =>
            {
                decimal refundAmount = OrderAmount.Value - Charges.Value;
                RaiseDomainEvent(new CancellationApprovedDomainEvent(Id, OrderId, refundAmount, remarks, approvedBy));
            })
            .Map(_ => Unit.Value);
    }

    /// <summary>
    /// Internal helper to encapsulate shared audit and state transitions.
    /// This prevents code duplication in Reject and Approve.
    /// </summary>
    private Result ApplyProcessingDetails(string remarks, Guid processedBy, CancellationStatus finalStatus)
    {
        var remarksResult = Remarks.Create(remarks);
        if (remarksResult.IsFailure) return remarksResult.TopError;

        if (processedBy == Guid.Empty)
            return DomainErrors.Cancellation.InvalidProcessedBy;

        // Apply all changes at once
        Remarks = remarksResult.Value;
        ProcessedBy = processedBy;
        ProcessedAt = DateTime.UtcNow;
        Status = finalStatus;

        return Result.Success();
    }

    // Keep this public if users need to change reason BEFORE rejection/approval
    public Result<Unit> UpdateReason(string newReason)
    {
        return Result.Success()
            .Ensure(_ => Status == CancellationStatus.Pending, DomainErrors.Cancellation.IsLocked)
            .Bind(_ => SetReason(newReason))
            .Map(_ => Unit.Value);
    }

    private Result<Unit> SetReason(string newReason)
    {
        var updatedReasonResult = Reason.Create(newReason);
        if (updatedReasonResult.IsFailure) return updatedReasonResult.TopError;

        Reason = updatedReasonResult.Value;
        return Unit.Value;
    }

    private Result<Unit> SetChaarges(decimal newCharges)
    {
        var chargesResult = Money.Create(newCharges);
        if (chargesResult.IsFailure) return chargesResult.TopError;

        Charges = chargesResult.Value;
        return Unit.Value;
    }
        

}
