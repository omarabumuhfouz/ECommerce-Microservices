namespace ProductService.Api.Contracts.Product;
public record AddDiscountRequest(int DiscountPercentage, DateTime DiscountEndDate);