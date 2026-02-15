namespace ProductService.Application.Features.Products.Commands.Tags.AddTag;

public record AddTagToProductCommand(Guid TagId, Guid ProductId) : ICommand<Unit>;