using AutoMapper;
using CancellationService.Application.Cancellations.Specifications;
using SharedKernel.Common;

namespace CancellationService.Application.Cancellations.Queries.GetCancellationById;

public class GetCancellationByIdQueryHandler : IQueryHandler<GetCancellationByIdQuery, CancellationDto>
{
    private readonly IUnitOfWork _unitOfWork;
   private readonly IMapper _mapper;

    public GetCancellationByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    async Task<Result<CancellationDto>> IRequestHandler<GetCancellationByIdQuery, Result<CancellationDto>>.Handle(GetCancellationByIdQuery request, CancellationToken ct)
    {
        return await _unitOfWork.GetRepository<Cancellation>()
            .FirstOrDefaultAsync(new GetCancellationByIdSpec(request.CancellationId), ct)
            .ToResult(DomainErrors.Cancellation.NotFound(request.CancellationId))
            .Map(cancellation => _mapper.Map<CancellationDto>(cancellation));
    }

}
