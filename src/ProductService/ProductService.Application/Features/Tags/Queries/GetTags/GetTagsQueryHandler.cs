using ProductService.Domain.Tags;

namespace ProductService.Application.Features.Tags.Queries.GetTags;

public class GetTagsQueryHandler : IQueryHandler<GetTagsQuery, List<TagDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetTagsQueryHandler> _logger;

    public GetTagsQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetTagsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async Task<Result<List<TagDto>>> IRequestHandler<GetTagsQuery, Result<List<TagDto>>>.Handle(GetTagsQuery request, CancellationToken cancellationToken)
    {
        var tagRepo = _unitOfWork.GetRepository<Tag>();

        var tags = await tagRepo.GetListAsync(cancellationToken);

        if (tags is null || !tags.Any())
        {
            _logger.LogWarning("No tags found in the database.");
            return new List<TagDto>();
        }

        _logger.LogInformation("Retrieved {TagCount} tags successfully.", tags.Count);

        return tags.Select(t => new TagDto(t.Id, t.Name)).ToList();
    }
}
