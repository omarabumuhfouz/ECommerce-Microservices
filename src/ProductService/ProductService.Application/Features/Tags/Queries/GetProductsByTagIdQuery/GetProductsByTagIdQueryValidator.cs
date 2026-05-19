namespace ProductService.Application.Features.Tags.Queries.GetProductsByTagIdQuery;

public sealed class GetProductsByTagIdQueryValidator : AbstractValidator<GetProductsByTagIdQuery>
{
    public GetProductsByTagIdQueryValidator()
    {
        RuleFor(x => x.TagId)
            .ValidateTagId();
    }
}