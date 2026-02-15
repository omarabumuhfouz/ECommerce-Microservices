namespace ProductService.Application.Features.Products.Commands.DeleteDiscount;
public class DeleteDiscountFromProductCommandValidator : AbstractValidator<DeleteDiscountFromProductCommand>
{
    public DeleteDiscountFromProductCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .ValidateProductId();
    }
}