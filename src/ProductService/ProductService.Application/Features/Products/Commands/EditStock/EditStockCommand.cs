using ProductService.Application.Enums;

namespace ProductService.Application.Features.Products.Commands.EditStock;

public record EditStockCommand(Guid ProductId, int Quantity, StockOperation operation) : ICommand<Unit>;