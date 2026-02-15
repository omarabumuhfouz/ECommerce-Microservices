namespace ProductService.Application.Features.Products.Commands.Tags.AddTag;

public class AddTagToProductCommandValidator : AbstractValidator<AddTagToProductCommand>
{
    public AddTagToProductCommandValidator()
    {
        RuleFor(c => c.ProductId)
            .ValidateProductId();
        
        RuleFor(c => c.TagId)
            .ValidateTagId();
    }
}