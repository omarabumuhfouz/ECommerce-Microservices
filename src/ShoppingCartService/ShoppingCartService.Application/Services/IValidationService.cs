namespace ShoppingCartService.Application.Services;

public interface IValidationService
{
    Result<Unit> EnsureSufficientStock(ProductDto product, int totalQuantity);
}