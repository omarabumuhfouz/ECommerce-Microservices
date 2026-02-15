namespace PaymentService.Application.Payments.Queries.GetPaymentById;

public record GetPaymentByIdQuery(Guid PaymentId) : IQuery<PaymentDto>;
