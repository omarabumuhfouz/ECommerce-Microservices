using Microsoft.Extensions.Logging;
using PaymentService.Application.Common.Enums;
using PaymentService.Application.Payments.specifications;
using PaymentService.Domain.Errors;
using PaymentService.Domain.Payments.Enums;

namespace PaymentService.Application.Payments.Commands.ProcessPayment;

public class ProcessPaymentCommandHandler : IQueryHandler<ProcessPaymentCommand, PaymentDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Payment> _paymentRepo;
    private readonly IOrderService _orderService;
    private readonly IValidationService _validation;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public ProcessPaymentCommandHandler(
        IUnitOfWork unitOfWork,
        IOrderService orderService,
        IValidationService validation,
        IMapper mapper,
        ILogger<ProcessPaymentCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _paymentRepo = _unitOfWork.GetRepository<Payment>();
        _orderService = orderService;
        _validation = validation;
        _mapper = mapper;
        _logger = logger;    
    }

    async Task<Result<PaymentDto>> IRequestHandler<ProcessPaymentCommand, Result<PaymentDto>>.Handle(ProcessPaymentCommand request, CancellationToken ct)
    {
        return  await _orderService.GetOrderByIdAsync(request.OrderId)

        .Bind(order => _validation.EnsurePaymentMatchesOrder(request.Amount, order.TotalAmount)
                .Map(() => order))

        .Bind(async order => await GetOrCreatePayment(request, order, ct))

        .Bind(async payment => await ProcessPaymentLogic(payment)
                .Map(_ => payment))

        .Tap(async _ => await _unitOfWork.SaveChangesAsync(ct))
        .Map(payment => _mapper.Map<PaymentDto>(payment))
        .TapError(err => _logger.LogError("Payment failed for Order {Id}: {Msg}", request.OrderId, err.Message));
    }

private async Task<Result<Payment>> GetOrCreatePayment(ProcessPaymentCommand request, OrderDto order, CancellationToken ct)
{
    var existingPayment = await _paymentRepo.GetSingleBySpecAsync(new GetPaymentByOrderIdSpec(request.OrderId), ct);

    if (existingPayment is not null)
    {
        if (existingPayment.Status == PaymentStatus.Failed && order.Status == OrderStatus.Pending)
        {
            existingPayment.UpdateDetails(request.PaymentMethod, request.Amount);
            existingPayment.UpdateStatus(PaymentStatus.Pending);
            return Result.Success(existingPayment);
        }

        return Result.Failure<Payment>(DomainErrors.Payment.NonRetriableState(existingPayment.Status.Name));
    }

    return Payment.Create(request.OrderId, request.PaymentMethod, request.Amount)
        .Tap(async p => await _paymentRepo.AddAsync(p));
}

    private async Task<Result<Unit>> ProcessPaymentLogic(Payment payment)
    {
        if (payment.IsCashOnDelivery()) return payment.MarkAsPending();

        return await SimulatePaymentGateway() == PaymentStatus.Completed
        ? payment.MarkAsCompleted(GenerateTransactionId())
        : payment.MarkAsFailed();
    }

    private async Task<PaymentStatus> SimulatePaymentGateway()
        => Random.Shared.Next(1, 101) <= 85 ? PaymentStatus.Completed : PaymentStatus.Failed;

    private string GenerateTransactionId() => $"TXN-{Guid.NewGuid().ToString("N").ToUpper().Substring(0, 12)}";




}
