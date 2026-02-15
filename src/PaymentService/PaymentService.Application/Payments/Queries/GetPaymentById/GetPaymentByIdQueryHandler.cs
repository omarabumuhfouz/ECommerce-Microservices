using PaymentService.Application.Payments.specifications;
using PaymentService.Domain.Errors;
using SharedKernel.Common;

namespace PaymentService.Application.Payments.Queries.GetPaymentById;

public class GetPaymentByIdQueryHandler : IQueryHandler<GetPaymentByIdQuery, PaymentDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPaymentByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    async Task<Result<PaymentDto>> IRequestHandler<GetPaymentByIdQuery, Result<PaymentDto>>.Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
        return  await _unitOfWork.GetRepository<Payment>()
                .GetSingleBySpecAsync(new GetPaymentByIdSpec(request.PaymentId), cancellationToken)
                .ToResult(DomainErrors.Payment.NotFound(request.PaymentId))
                .Map(payment => _mapper.Map<PaymentDto>(payment));
    }
}
