namespace ProductService.Application.Features.Products.Queries.GetProductNames;
public class GetProductsByIdsQueryValidator : AbstractValidator<GetProductNamesQuery>
{
    public GetProductsByIdsQueryValidator()
    {
        RuleFor(x => x.ProductIds)
            .NotNull().WithMessage("Product IDs list cannot be null.")
            .NotEmpty().WithMessage("At least one product ID must be provided.");
    }
}
