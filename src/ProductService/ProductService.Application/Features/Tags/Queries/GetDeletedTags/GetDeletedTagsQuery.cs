namespace ProductService.Application.Features.Tags.Queries.GetDeletedTags;

public sealed record GetDeletedTagsQuery() : ICommand<List<TagDto>>;