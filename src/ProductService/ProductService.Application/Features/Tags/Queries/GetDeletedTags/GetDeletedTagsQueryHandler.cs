using ProductService.Application.Features.Tags.Specifications;
using ProductService.Domain.Tags;

namespace ProductService.Application.Features.Tags.Queries.GetDeletedTags;

public sealed class GetDeletedTagsQueryHandler : ICommandHandler<GetDeletedTagsQuery, List<TagDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetDeletedTagsQueryHandler> _logger;

    public GetDeletedTagsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetDeletedTagsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<List<TagDto>>> Handle(GetDeletedTagsQuery request, CancellationToken ct)
    {
        var tags = await _unitOfWork.GetRepository<Tag>().GetListAsync(new GetDeletedTagsSpec(), ct);

        return Result.Success(tags ?? [])
            .Tap(list => 
            {
                if (list.Count == 0) _logger.LogInformation("No deleted tags found in the system.");
                else _logger.LogInformation("Successfully retrieved {Count} deleted tags.", list.Count);
            });
    }
}