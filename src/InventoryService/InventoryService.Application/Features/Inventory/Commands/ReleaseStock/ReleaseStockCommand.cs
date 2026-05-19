namespace InventoryService.Application.Features.Inventory.Commands.ReleaseStock;

public record ReleaseStockCommand(Guid ProductId, Guid OrderId) : ICommand<Unit>;