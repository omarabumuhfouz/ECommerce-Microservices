
using OrderService.Domain.Errors;

namespace OrderService.Domain.Services;

public class ValidationService : IValidationService
{
    public Result ValidateStock(ProductDto product, int requestedQuantity, int existingQuantity = 0)
    {
        if (requestedQuantity <= 0) return DomainErrors.Order.InvalidQuantity(product.Name);

        var availableStock = product.StockQuantity + existingQuantity;

        if (availableStock < requestedQuantity)
        {
            return Result.Failure( Error.Validation(
                "Product.InsufficientStock", 
                $"Insufficient stock for '{product.Name}'. Available: {availableStock}, Requested: {requestedQuantity}"));
        }

        return Result.Success();
    }
}
