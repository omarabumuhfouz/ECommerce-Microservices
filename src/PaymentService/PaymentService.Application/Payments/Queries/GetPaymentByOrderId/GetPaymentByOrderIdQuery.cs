namespace PaymentService.Application.Payments.Queries.GetPaymentByOrderId;

public record GetPaymentByOrderIdQuery(Guid OrderId) : IQuery<PaymentDto>;
