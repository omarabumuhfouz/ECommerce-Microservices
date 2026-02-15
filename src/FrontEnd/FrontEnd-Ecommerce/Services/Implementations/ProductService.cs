
namespace FrontEnd_Ecommerce.Services.Implementations;

public class ProductService : IProductService
{
    public List<Product> GetProducts()
    {
        return new List<Product>
        {
            new Product
            {
                Name = "Wireless Headphones",
                ImageUrl = "https://images.unsplash.com/photo-1546868871-7041f2a55e12?ixlib=rb-4.0.3&auto=format&fit=crop&w=400&q=80",
                Price = 79.99m,
                OldPrice = 99.99m,
                Badge = "-20%",
                Rating = 4.5
            },
            new Product
            {
                Name = "Digital Camera",
                ImageUrl = "https://images.unsplash.com/photo-1526170375885-4d8ecf77b99f?ixlib=rb-4.0.3&auto=format&fit=crop&w=400&q=80",
                Price = 249.99m,
                Badge = null,
                Rating = 5
            },
            new Product
            {
                Name = "Smart Watch",
                ImageUrl = "https://images.unsplash.com/photo-1585386959984-a4155224a1ad?ixlib=rb-4.0.3&auto=format&fit=crop&w=400&q=80",
                Price = 129.99m,
                Badge = "New",
                Rating = 4
            },
            new Product
            {
                Name = "Wireless Earbuds",
                ImageUrl = "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?ixlib=rb-4.0.3&auto=format&fit=crop&w=400&q=80",
                Price = 59.99m,
                Badge = null,
                Rating = 3.5
            },
            new Product
            {
                Name = "Ultrabook Laptop",
                ImageUrl = "https://images.unsplash.com/photo-1541807084-5c52b6b3adef?ixlib=rb-4.0.3&auto=format&fit=crop&w=400&q=80",
                Price = 899.99m,
                OldPrice = 1059.99m,
                Badge = "-15%",
                Rating = 4.5
            },
            new Product
            {
                Name = "Gaming Mouse",
                ImageUrl = "https://images.unsplash.com/photo-1531297484001-80022131f5a1?ixlib=rb-4.0.3&auto=format&fit=crop&w=400&q=80",
                Price = 49.99m,
                Badge = null,
                Rating = 4
            }
        };
    }
}
