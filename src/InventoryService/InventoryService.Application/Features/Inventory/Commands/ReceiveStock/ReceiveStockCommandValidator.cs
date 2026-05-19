using FluentValidation;

namespace InventoryService.Application.Features.Inventory.Commands.ReceiveStock;

public sealed class ReceiveStockCommandValidator : AbstractValidator<ReceiveStockCommand>
{
    public ReceiveStockCommandValidator()
    {
        RuleFor(c => c.ProductId).NotEmpty();
        RuleFor(c => c.Amount).GreaterThan(0);
    }
}