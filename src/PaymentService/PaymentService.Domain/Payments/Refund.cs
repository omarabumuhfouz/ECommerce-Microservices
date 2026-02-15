﻿using SharedKernel.Shared;
using SharedKernel.Primitives;
using PaymentService.Domain.Payments.ValueObjects;
using PaymentService.Domain.Errors;
using PaymentService.Domain.Payments.Enums;

namespace PaymentService.Domain.Payments;

public record Refund : Entity
{
    private Refund() { }

    private Refund(
        Guid cancellationId,
        Guid paymentId,
        Guid orderId,
        Money amount,
        PaymentMethod refundMethod,
        Reason? refundReason,
        TransactionId? transactionId) : base(Guid.Empty)
    {
        CancellationId = cancellationId;
        PaymentId = paymentId;
        OrderId = orderId;
        Amount = amount;
        Method = refundMethod;
        Reason = refundReason;
        TransactionId = transactionId;

        Status = RefundStatus.Pending;
        InitiatedAt = DateTime.UtcNow;
    }

    public Guid CancellationId { get; private set; }
    public Guid PaymentId { get; private set; }
    public Payment Payment { get; private set; } = null!; 
    public Guid OrderId { get; private set; }
    public PaymentMethod Method { get; private set; }

    public Money Amount { get; private set; }
    public RefundStatus Status { get; private set; }
    public Reason? Reason { get; private set; }
    public TransactionId? TransactionId { get; private set; }

    public DateTime InitiatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public Guid? ProcessedBy { get; private set; } 

    
    public static Result<Refund> Create(
        Guid cancellationId,
        Guid paymentId,
        Guid orderId,
        PaymentMethod method,
        decimal amount,
        string? refundReason = null,
        string? transactionId = null)
    {
        // Make Required Validation

        
        var moneyResult = Money.Create(amount);
        if (moneyResult.IsFailure) return moneyResult.TopError;

        Result<Reason>? reasonResult = null;
        {
            reasonResult = Reason.Create(refundReason);
            if (reasonResult.IsFailure) return reasonResult.TopError;
        }

        Result<TransactionId>? transactionResult = null;
        if (!string.IsNullOrEmpty(transactionId))
        {
            transactionResult = TransactionId.Create(transactionId);
            if (transactionResult.IsFailure) return transactionResult.TopError;
        }

        var refund = new Refund(
            cancellationId,
            paymentId,
            orderId,
            moneyResult.Value,
            method,
            reasonResult?.Value,
            transactionResult?.Value
        );

        return refund;
    }

    public Result MarkAsCompleted(DateTime completedAt, Guid processedBy)
    {
        if (Status == RefundStatus.Completed) return Result.Success();

        if (completedAt < InitiatedAt) return DomainErrors.Refund.InvalidCompletionDate;

        CompletedAt = completedAt;
        ProcessedBy = processedBy;
        Status = RefundStatus.Completed;

        return Result.Success();
    }

    public void MarkAsFailed()
    {
        if (Status != RefundStatus.Completed) Status = RefundStatus.Failed;
    }
    
    public Result SetTransactionId(string? transactionId)
    {
        var txResult = TransactionId.Create(transactionId);
        if (txResult.IsFailure) return txResult.TopError;

        TransactionId = txResult.Value;
        return Result.Success();
    }
}