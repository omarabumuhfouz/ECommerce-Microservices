using PaymentService.Application.Payments.specifications;
using PaymentService.Domain.Errors;
using SharedKernel.Common;

namespace PaymentService.Application.Payments.Queries.GetPaymentByOrderId;

public class GetPaymentByOrderIdQueryHandler : IQueryHandler<GetPaymentByOrderIdQuery, PaymentDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPaymentByOrderIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    async Task<Result<PaymentDto>> IRequestHandler<GetPaymentByOrderIdQuery, Result<PaymentDto>>.Handle(GetPaymentByOrderIdQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.GetRepository<Payment>()
                .GetSingleBySpecAsync(new GetPaymentByOrderIdSpec(request.OrderId), cancellationToken)
                .ToResult(DomainErrors.Payment.NotFoundForOrder(request.OrderId))
                .Map(payment => _mapper.Map<PaymentDto>(payment));
    }
}
