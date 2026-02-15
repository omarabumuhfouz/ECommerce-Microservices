namespace OrderService.Domain.Services;

public interface IValidationService
{
    Result ValidateStock(ProductDto product, int requestedQuantity, int existingQuantity = 0);
}
