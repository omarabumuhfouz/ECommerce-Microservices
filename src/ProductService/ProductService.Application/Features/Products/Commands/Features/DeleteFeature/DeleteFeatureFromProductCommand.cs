namespace ProductService.Application.Features.Products.Commands.Features.DeleteFeature;
public record DeleteFeatureFromProductCommand(Guid ProductId, string Name) : ICommand<Unit>;