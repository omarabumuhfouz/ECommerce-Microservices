using ProductService.Domain.Tags;

namespace ProductService.Application.Features.Tags.Queries.SearchTags;

public sealed class SearchTagsSpec : Specification<Tag, TagDto>
{
    public SearchTagsSpec(string searchTerm)
    {
        Query.AsNoTracking()
             .Where(tag => tag.Name.Contains(searchTerm))
             .OrderBy(tag => tag.Name) 
             .Take(10) 
             .Select(tag => new TagDto(tag.Id, tag.Name));
    }
}