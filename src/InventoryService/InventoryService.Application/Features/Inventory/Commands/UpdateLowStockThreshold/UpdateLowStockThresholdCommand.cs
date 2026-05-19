namespace InventoryService.Application.Features.Inventory.Commands.UpdateLowStockThreshold;

public record UpdateLowStockThresholdCommand(Guid ProductId, int NewThreshold) : ICommand<Unit>;