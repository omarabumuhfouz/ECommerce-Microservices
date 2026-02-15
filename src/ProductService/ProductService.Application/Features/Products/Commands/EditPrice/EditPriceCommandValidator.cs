namespace ProductService.Application.Features.Products.Commands.EditPrice;
public class EditPriceCommandValidator : AbstractValidator<EditPriceCommand>
{
    public EditPriceCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .ValidateProductId();

        RuleFor(x => x.NewPrice)
            .ValidatePrice();

        RuleFor(x => x.NewCurrency)
            .ValidateCurrency();
    }
}
