namespace InventoryService.Application.Features.Inventory.Queries.GetBatchStockAvailability;

public sealed class StockRequestItemValidator : AbstractValidator<StockRequestItem>
{
    public StockRequestItemValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required.");

        RuleFor(x => x.RequestedQuantity)
            .GreaterThan(0).WithMessage("Requested quantity must be at least 1.");
    }
}