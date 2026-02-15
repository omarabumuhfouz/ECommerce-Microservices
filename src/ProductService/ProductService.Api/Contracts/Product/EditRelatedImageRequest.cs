namespace ProductService.Api.Contracts.Product;

public record EditRelatedImageRequest(string OldUrl,string NewUrl, string? NewAltText);