using FluentValidation;

namespace InventoryService.Application.Features.Inventory.Commands.DispatchStock;

public sealed class DispatchStockCommandValidator : AbstractValidator<DispatchStockCommand>
{
    public DispatchStockCommandValidator()
    {
        RuleFor(c => c.ProductId).NotEmpty();
        RuleFor(c => c.OrderId).NotEmpty();
    }
}