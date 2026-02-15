namespace ProductService.Application.Features.Products.Commands.EditProductStatus;

public class EditProductStatusCommandValidator : AbstractValidator<EditProductStatusCommand>
{
    public EditProductStatusCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .ValidateProductId();

        RuleFor(x => x.IsAvailable)
             .NotEmpty()
                .WithMessage("Product status is required.");
    }
}