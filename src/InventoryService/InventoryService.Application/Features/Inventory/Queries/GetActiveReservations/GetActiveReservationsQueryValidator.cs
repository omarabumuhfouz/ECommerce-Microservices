namespace InventoryService.Application.Features.Inventory.Queries.GetActiveReservations;

/// <summary>
/// Validator for the <see cref="GetActiveReservationsQuery"/>.
/// </summary>
public sealed class GetActiveReservationsQueryValidator : AbstractValidator<GetActiveReservationsQuery>
{
    public GetActiveReservationsQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID cannot be empty.");
    }
}
