namespace ProductService.Api.Contracts.Product;

public record EditFeatureRequest(string OldName, string NewName, string NewValue);