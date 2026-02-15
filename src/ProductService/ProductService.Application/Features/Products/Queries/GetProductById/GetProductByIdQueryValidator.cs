namespace ProductService.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdQueryValidator()
    {
        RuleFor(c => c.ProductId)
            .ValidateProductId();
    }
}