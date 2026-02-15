namespace PaymentService.Application.Services;

public interface IPaymentGateway
{
    Task<PaymentGatewayRefundDto> RefundAsync(decimal amount, CancellationToken cancellationToken = default);
}