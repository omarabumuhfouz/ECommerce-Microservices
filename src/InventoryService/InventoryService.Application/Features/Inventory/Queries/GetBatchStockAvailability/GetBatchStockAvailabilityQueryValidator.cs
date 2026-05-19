namespace InventoryService.Application.Features.Inventory.Queries.GetBatchStockAvailability;

public sealed class GetBatchStockAvailabilityQueryValidator : AbstractValidator<GetBatchStockAvailabilityQuery>
{
    public GetBatchStockAvailabilityQueryValidator()
    {
        RuleFor(x => x.Items)
            .NotNull()
            .NotEmpty().WithMessage("At least one item must be requested.");

        // Clean & Powerful: Validates every single item in the list
        RuleForEach(x => x.Items)
            .SetValidator(new StockRequestItemValidator());
    }
}
