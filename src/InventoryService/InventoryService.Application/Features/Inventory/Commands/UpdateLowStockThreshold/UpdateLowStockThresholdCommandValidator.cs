namespace InventoryService.Application.Features.Inventory.Commands.UpdateLowStockThreshold;

public sealed class UpdateLowStockThresholdCommandValidator : AbstractValidator<UpdateLowStockThresholdCommand>
{
    public UpdateLowStockThresholdCommandValidator()
    {
        RuleFor(c => c.ProductId).NotEmpty();
        RuleFor(c => c.NewThreshold).GreaterThanOrEqualTo(0);
    }
}