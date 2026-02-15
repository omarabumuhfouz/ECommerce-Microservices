namespace ProductService.Application.Features.Products.Queries.GetProductsByIds;

public class GetProductsByIdsQueryValidator : AbstractValidator<GetProductsByIdsQuery>
{
    public GetProductsByIdsQueryValidator()
    {
        RuleFor(x => x.ProductIds)
            .NotEmpty()
            .WithMessage("Product IDs list cannot be empty.");

        RuleForEach(x => x.ProductIds)
            .NotEmpty()
            .WithMessage("Product ID cannot be empty.");
    }
}
