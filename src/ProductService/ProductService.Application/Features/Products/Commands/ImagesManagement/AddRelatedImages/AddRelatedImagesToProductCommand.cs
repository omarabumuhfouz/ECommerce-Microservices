using ProductService.Application.Features.Products.DTOs;

namespace ProductService.Application.Features.Products.Commands.ImagesManagement.AddRelatedImages;

public record AddRelatedImagesToProductCommand(
     Guid ProductId,
     List<ImageDto> RelatedImages) 

: ICommand<Unit>;