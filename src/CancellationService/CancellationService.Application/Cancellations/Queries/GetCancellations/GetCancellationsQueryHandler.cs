using AutoMapper;
using CancellationService.Application.Cancellations.Specifications;

namespace CancellationService.Application.Cancellations.Queries.GetCancellations;

public class GetCancellationsQueryHandler : IQueryHandler<GetCancellationsQuery, PagedList<CancellationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Cancellation> _repo;
    private readonly IMapper _mapper;

    public GetCancellationsQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _unitOfWork = unitOfWork;
        _repo = _unitOfWork.GetRepository<Cancellation>();
        _mapper = mapper;
    }

    async Task<Result<PagedList<CancellationDto>>> IRequestHandler<GetCancellationsQuery, Result<PagedList<CancellationDto>>>.Handle(GetCancellationsQuery request, CancellationToken ct)
    {
        var cancellations = await _repo
            .GetListAsync(new GetCancellationsSpec
                (
                    request.PageNumber,
                    request.PageSize,
                    request.SortBy,
                    request.IsAscending,
                    request.Status,
                    request.SearchTerm
                ), ct);

        if (!cancellations.Any()) return PagedList<CancellationDto>.Empty();

        return new PagedList<CancellationDto>(
            _mapper.Map<List<CancellationDto>>(cancellations),
            request.PageNumber,
            request.PageSize,
            await _repo.CountAsync(ct)
        );
    }
}
