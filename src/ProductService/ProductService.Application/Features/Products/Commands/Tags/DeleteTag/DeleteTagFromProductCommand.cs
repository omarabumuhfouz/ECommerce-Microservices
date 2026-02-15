namespace ProductService.Application.Features.Products.Commands.Tags.DeleteTag;
public record DeleteTagFromProductCommand(Guid TagId, Guid ProductId) : ICommand<Unit>;