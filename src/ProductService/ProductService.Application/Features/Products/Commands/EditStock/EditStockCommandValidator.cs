namespace ProductService.Application.Features.Products.Commands.EditStock;
public class EditStockCommandValidator : AbstractValidator<EditStockCommand>
{
    public EditStockCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .ValidateProductId();

        RuleFor(x => x.Quantity)
            .ValidateStockQuantity();

        RuleFor(x => x.operation)
            .IsInEnum().WithMessage("Invalid stock operation. Must be 'Increase' or 'Decrease'.");
    }
}