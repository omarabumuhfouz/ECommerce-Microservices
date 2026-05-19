using ProductService.Domain.Tags;

namespace ProductService.Application.Features.Tags.Queries.SearchTags;

public sealed class SearchTagsQueryHandler : ICommandHandler<SearchTagsQuery, List<TagDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SearchTagsQueryHandler> _logger;

    public SearchTagsQueryHandler(IUnitOfWork unitOfWork, ILogger<SearchTagsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<List<TagDto>>> Handle(SearchTagsQuery request, CancellationToken ct)
    {
        var tags = await _unitOfWork.GetRepository<Tag>().GetListAsync(new SearchTagsSpec(request.SearchTerm), ct);

        return Result.Success(tags ?? [])
            .Tap(list => _logger.LogInformation(
                "Search for '{SearchTerm}' yielded {Count} results.", request.SearchTerm, list.Count));
    }
}