namespace InventoryService.Application.Features.Inventory.Commands.AdjustInventory;

public sealed class AdjustInventoryCommandValidator : AbstractValidator<AdjustInventoryCommand>
{
    public AdjustInventoryCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required.");

        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Order ID is required.");

        RuleFor(x => x.QuantityDelta)
            .NotEqual(0).WithMessage("Adjustment quantity cannot be zero.");
    }
}
