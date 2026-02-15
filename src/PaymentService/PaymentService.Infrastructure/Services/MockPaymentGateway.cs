using Microsoft.Extensions.Logging;
using PaymentService.Application.DTOs;
using PaymentService.Application.Services;
using PaymentService.Domain.Payments.Enums;
using SharedKernel.Shared;

namespace RefundService.Infrastructure.Services;

public class MockPaymentGateway : IPaymentGateway
{
    private readonly ILogger<MockPaymentGateway> _logger;

    public MockPaymentGateway(ILogger<MockPaymentGateway> logger)
    {
        _logger = logger;
    }

    public async Task<PaymentGatewayRefundDto> RefundAsync(decimal amount, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("MOCK GATEWAY: Starting refund process for {Amount:C}...", amount);

        await Task.Delay(10, cancellationToken);

        var random = new Random();
        double chance = random.NextDouble();

        if (chance < 0.90)
        {
            var txnId = $"TXN-{Guid.NewGuid().ToString()[..8].ToUpper()}";

            _logger.LogInformation("MOCK GATEWAY: Success! Generated TxnId: {TxnId}", txnId);

            return new PaymentGatewayRefundDto
            (
                IsSuccess: true,
                Status: RefundStatus.Completed,
                TransactionId: txnId 
            );
        }

        _logger.LogWarning("MOCK GATEWAY: Simulating failure for amount {Amount}", amount);

        return new PaymentGatewayRefundDto
        (
            IsSuccess: false,
            Status: RefundStatus.Failed,
            TransactionId: null
        );
    }
}