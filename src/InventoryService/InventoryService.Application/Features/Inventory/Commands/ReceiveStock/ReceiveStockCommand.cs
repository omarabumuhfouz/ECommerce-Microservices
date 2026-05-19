namespace InventoryService.Application.Features.Inventory.Commands.ReceiveStock;

public record ReceiveStockCommand(Guid ProductId, int Amount) : ICommand<Unit>;