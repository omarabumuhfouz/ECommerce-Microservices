namespace ProductService.Application.Features.Products.Commands.ImagesManagement.ReplaceMainImage;

public record ReplaceMainImageCommand(Guid ProductId, string NewMainUrl, string? NewTextAlt) : ICommand<Unit>;