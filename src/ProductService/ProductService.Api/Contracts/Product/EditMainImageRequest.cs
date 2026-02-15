namespace ProductService.Api.Contracts.Product;

public record EditMainImageRequest(string NewMainUrl, string? NewAltText);