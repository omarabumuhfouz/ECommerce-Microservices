namespace ProductService.Application.Features.Tags.Queries.SearchTags;
public sealed record SearchTagsQuery(string SearchTerm) : ICommand<List<TagDto>>;