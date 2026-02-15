namespace ProductService.Application.Features.Products.Commands.EditProduct;

public class EditProductCommandValidator : AbstractValidator<EditProductCommand>
{
    public EditProductCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .ValidateProductId();

        RuleFor(x => x.Name)
            .ValidateProductName();

        RuleFor(x => x.Description)
            .ValidateProductDescription();
    }
}