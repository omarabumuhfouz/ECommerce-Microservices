namespace InventoryService.Application.Features.Inventory.Commands.AdjustInventory;

public sealed record AdjustInventoryCommand(
    Guid ProductId,
    Guid OrderId,
    int QuantityDelta
) : ICommand<Unit>;
