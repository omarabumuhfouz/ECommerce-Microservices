
using PaymentService.Domain.Payments.Enums;

namespace PaymentService.Application.DTOs;

public record PaymentGatewayRefundDto(bool IsSuccess, RefundStatus Status, string? TransactionId = null);