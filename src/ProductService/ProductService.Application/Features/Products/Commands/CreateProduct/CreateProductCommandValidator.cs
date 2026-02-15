namespace ProductService.Application.Features.Products.Commands.CreateProduct;
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .ValidateProductName();

        RuleFor(x => x.Description)
            .ValidateProductDescription();

        RuleFor(x => x.Price)
            .ValidatePrice();

        RuleFor(x => x.Currency)
            .ValidateCurrency();

        RuleFor(x => x.StockQuantity)
            .ValidateStockQuantity();

        RuleFor(x => x.MainImage.Url)
            .ValidateImageUrl();

        RuleFor(x => x.DiscountPercentage)
            .ValidateDiscountPercentage();

        RuleFor(x => x.CategoryId)
             .ValidateCategoryId();
    }
}
