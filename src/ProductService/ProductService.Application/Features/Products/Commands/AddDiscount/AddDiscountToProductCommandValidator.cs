namespace ProductService.Application.Features.Products.Commands.AddDiscount;

public class AddDiscountToProductCommandValidator : AbstractValidator<AddDiscountToProductCommand>
{
    public AddDiscountToProductCommandValidator()
    {
        RuleFor(c => c.ProductId)
            .ValidateProductId();

        RuleFor(c => c.DiscountEndDate)
            .ValidateDiscountEndDate();

        RuleFor(c => c.DiscountPercentage)
            .ValidateDiscountPercentage();
    
        
    }
}