namespace ProductService.Application.Features.Products.Commands.EditProductStatus;

public record EditProductStatusCommand(Guid ProductId, bool IsAvailable) : ICommand<Unit>;