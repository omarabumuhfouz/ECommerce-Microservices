using AutoMapper;
using CancellationService.Application.Cancellations.Specifications;
using SharedKernel.Repositories;

namespace CancellationService.Application.Cancellations.Queries.GetCancellationByOrderId;

public class GetCancellationByOrderIdQueryHandler : ICommandHandler<GetCancellationByOrderIdQuery, CancellationDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCancellationByOrderIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CancellationDto>> Handle(GetCancellationByOrderIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new GetCancellationByOrderIdSpec(request.OrderId);

        return await _unitOfWork.GetRepository<Cancellation>()
            .FirstOrDefaultAsync(spec, cancellationToken)
            .ToResult(DomainErrors.Cancellation.NotFoundForOrder(request.OrderId))
            .Map(cancellation => _mapper.Map<CancellationDto>(cancellation));
    }
}