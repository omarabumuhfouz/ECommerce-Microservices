using Contracts.Product;

namespace ShoppingCartService.Application.DTOs;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int DiscountPercentage { get; set; }

    public static ProductDto FromGrpcModel(ProductModel model)
    {
        return new ProductDto
        {
            Id = Guid.Parse(model.Id),
            Name = model.Name,
            Price = decimal.Parse(model.Price),
            StockQuantity = model.StockQuantity,
            DiscountPercentage = model.DiscountPercentage
        };
        
    }

    /// <summary>
    /// Calculates the discount amount for a given product based on its discount percentage.
    /// </summary>
    /// <param name="product">The product for which to calculate the discount.</param>
    /// <returns>
    /// The calculated discount amount as a decimal. 
    /// Returns 0 if the product has no discount (DiscountPercentage is 0 or less).
    /// </returns>
    public decimal CalculateDiscount()
    {
        return DiscountPercentage > 0 ? Price * DiscountPercentage / 100 : 0;
    }

}

