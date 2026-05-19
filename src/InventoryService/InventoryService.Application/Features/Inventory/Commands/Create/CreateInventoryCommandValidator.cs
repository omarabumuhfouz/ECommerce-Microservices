namespace InventoryService.Application.Features.Inventory.Commands.Create;

public sealed class CreateInventoryCommandValidator : AbstractValidator<CreateInventoryCommand>
{
    public CreateInventoryCommandValidator()
    {
        RuleFor(c => c.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required.");

        RuleFor(c => c.InitialStock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Initial stock cannot be negative.");
    }
}