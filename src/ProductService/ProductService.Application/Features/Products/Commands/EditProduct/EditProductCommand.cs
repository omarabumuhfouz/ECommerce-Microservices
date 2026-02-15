namespace ProductService.Application.Features.Products.Commands.EditProduct;
public record EditProductCommand(
    Guid ProductId,
    string Name,
    string Description

) : ICommand<Unit>;
