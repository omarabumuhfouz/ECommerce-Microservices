using ProductService.Domain.Tags;

namespace ProductService.Application.Features.Tags.Specifications;

public sealed class GetDeletedTagsSpec : Specification<Tag, TagDto>
{
    public GetDeletedTagsSpec()
    {
        Query.AsNoTracking()
             .IgnoreQueryFilters() 
             .Where(tag => tag.IsDeleted)
             .Select(tag => new TagDto(tag.Id, tag.Name)); 
    }
}