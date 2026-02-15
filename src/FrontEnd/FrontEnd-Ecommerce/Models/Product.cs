namespace FrontEnd_Ecommerce.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public string Badge { get; set; } // e.g., "New" or "-20%"
        public double Rating { get; set; } // 0-5
    }
}
