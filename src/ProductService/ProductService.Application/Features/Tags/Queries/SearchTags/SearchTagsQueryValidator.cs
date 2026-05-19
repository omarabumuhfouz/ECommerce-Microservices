namespace ProductService.Application.Features.Tags.Queries.SearchTags;

public sealed class SearchTagsQueryValidator : AbstractValidator<SearchTagsQuery>
{
    public SearchTagsQueryValidator()
    {
        RuleFor(x => x.SearchTerm)
            .NotEmpty().WithMessage("Search term cannot be empty.")
            .MinimumLength(2).WithMessage("Please enter at least 2 characters to search.");
    }
}