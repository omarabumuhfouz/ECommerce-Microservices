namespace InventoryService.Application.Features.Inventory.Queries.GetStockAvailability;

/// <summary>
/// Validator for the <see cref="GetStockAvailabilityQuery"/>.
/// </summary>
public sealed class GetStockAvailabilityQueryValidator : AbstractValidator<GetStockAvailabilityQuery>
{
    public GetStockAvailabilityQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID cannot be empty.");

        RuleFor(x => x.RequestedQuantity)
            .GreaterThan(0)
            .WithMessage("Requested quantity must be greater than zero.");
    }
}
