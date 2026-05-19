namespace ProductService.Application.Features.Products.Commands.RestoreProduct;

public sealed record RestoreProductCommand(Guid ProductId) : ICommand<Unit>;