using Contracts.Product;

namespace OrderService.Domain.DTOs;

public class ProductDto{
    public Guid Id { get; set; }
    public int StockQuantity { get; set; }
    public string Name { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal Price { get; set; }

    public static ProductDto FromGrpcModel(ProductModel model)
    {
        return new()
        {
            Id = Guid.Parse(model.Id),
            StockQuantity = model.StockQuantity,
            Name = model.Name,
            DiscountPercentage = model.DiscountPercentage,
            Price = Convert.ToDecimal(model.Price)
        };
    }

}
