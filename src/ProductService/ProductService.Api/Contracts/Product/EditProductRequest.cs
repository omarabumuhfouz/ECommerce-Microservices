namespace ProductService.Api.Contracts.Product;

public record EditProductRequest(
    string Name, 
    string Description
);