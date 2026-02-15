namespace ShoppingCartService.Application.Services;

public class ValidationService : IValidationService
{
    public Result<Unit> EnsureSufficientStock(ProductDto product, int totalQuantity)
    {
        if (totalQuantity > product.StockQuantity) return DomainErrors.Product.InSufficientStock;

        return Unit.Value;
    }
}
