﻿using Microsoft.Extensions.Logging;
using PaymentService.Application.Payments.specifications;
using PaymentService.Domain.Errors;
using SharedKernel.Common;

namespace PaymentService.Application.Payments.Commands.CompleteCodPayment;

public class CompleteCodPaymentCommandHandler : ICommandHandler<CompleteCodPaymentCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Payment> _paymentRepo;
    private readonly IOrderService _orderService;
    private readonly IValidationService _validation;
    private readonly ILogger<CompleteCodPaymentCommandHandler> _logger;

    public CompleteCodPaymentCommandHandler(
        IUnitOfWork unitOfWork,
        IOrderService orderService,
        IValidationService validation,
        ILogger<CompleteCodPaymentCommandHandler> logger 
    )
    {
        _unitOfWork = unitOfWork;
        _paymentRepo = unitOfWork.GetRepository<Payment>();
        _orderService = orderService;
        _validation = validation;
        _logger = logger;
    }

    async Task<Result<Unit>> IRequestHandler<CompleteCodPaymentCommand, Result<Unit>>.Handle(CompleteCodPaymentCommand request, CancellationToken ct)
    {
        return await GetCompleteCodePaymentContext(request.PaymentId, request.OrderId, ct)
                //Validate Order Status
            .Bind(ctx => _validation.EnsureReadyForDelivery(ctx.Order.Status)
                .TapError(err => _logger.LogWarning("Order {Id} not ready. Status: {Status}", ctx.Order.Id, ctx.Order.Status))
                .Map(() => ctx)) // Keep the context flowing

            .Bind(ctx => ctx.Payment.EnsurePaymentIsCashOnDelivery()
                .TapError(err => _logger.LogWarning("Payment {Id} is not COD.", ctx.Payment.Id))
                .Map(_ => ctx))

            .Bind(ctx => ctx.Payment.MarkAsCompleted()
                .Map(_ => ctx))

            .Tap(async _ => await _unitOfWork.SaveChangesAsync(ct))
            .Tap(_ => _logger.LogInformation("Successfully completed COD payment {Id}.", request.PaymentId))
            .Map(_ => Unit.Value);
    }

    private async Task<Result<(Payment Payment, OrderDto Order)>> GetCompleteCodePaymentContext(Guid paymentId, Guid orderId, CancellationToken ct)
    {   
        return await _paymentRepo
        .GetSingleBySpecAsync(new GetPaymentByIdAndOrderIdSpec(paymentId, orderId, true), ct)
        .ToResult(DomainErrors.Payment.NotFound(paymentId, orderId))
        .Bind(async payment => 
        {
            var orderResult = await _orderService.GetOrderByIdAsync(payment.OrderId);
            return orderResult.Map(order => (payment, order));
        });
    }
}
