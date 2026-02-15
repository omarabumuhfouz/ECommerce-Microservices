namespace ProductService.Application.Features.Products.Commands.ImagesManagement.DeleteRelatedImage;
public record DeleteRelatedImageFromProductCommand(Guid ProductId, string Url) : ICommand<Unit>;
