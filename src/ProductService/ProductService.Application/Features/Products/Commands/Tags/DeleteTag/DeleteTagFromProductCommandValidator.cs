namespace ProductService.Application.Features.Products.Commands.Tags.DeleteTag;

public class DeleteTagFromProductCommandValidator : AbstractValidator<DeleteTagFromProductCommand>
{
    public DeleteTagFromProductCommandValidator()
    {
        RuleFor(c => c.ProductId)
            .ValidateProductId();
        
        RuleFor(c => c.TagId)
            .ValidateTagId();
    }
}