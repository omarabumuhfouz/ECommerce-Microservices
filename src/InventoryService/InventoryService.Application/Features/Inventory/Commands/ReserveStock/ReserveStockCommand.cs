namespace InventoryService.Application.Features.Inventory.Commands.ReserveStock;

public record ReserveStockCommand(Guid ProductId, Guid OrderId, int Amount) : ICommand<Unit>;