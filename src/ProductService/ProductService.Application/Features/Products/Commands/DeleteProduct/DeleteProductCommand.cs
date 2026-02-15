namespace ProductService.Application.Features.Products.Commands.DeleteProduct;

public record DeleteProductCommand(Guid ProductId) : ICommand<Unit>;
