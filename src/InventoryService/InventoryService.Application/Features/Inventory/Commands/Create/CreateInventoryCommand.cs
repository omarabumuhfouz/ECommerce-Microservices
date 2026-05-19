namespace InventoryService.Application.Features.Inventory.Commands.Create;

public record CreateInventoryCommand(Guid ProductId, int InitialStock, int LowStockThreshold) : ICommand<Guid>;