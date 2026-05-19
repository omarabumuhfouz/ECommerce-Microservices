namespace InventoryService.Application.Features.Inventory.Commands.DispatchStock;

public record DispatchStockCommand(Guid ProductId, Guid OrderId) : ICommand<Unit>;