namespace ShoppingCartService.Application.Carts.DTOs;

public class CartDto
{
    public Guid Id { get; set; }
    public Guid? CustomerId { get; set; }
    public bool IsCheckedOut { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public decimal TotalBasePrice { get; set; }
    public decimal TotalDiscount { get; set; }
    public decimal TotalAmount { get; set; }
    public List<CartItemDto>? CartItems { get; set; }


/// <summary>
    /// Creates a default empty cart for a specific customer.
    /// </summary>
    public static CartDto DefaultCart(Guid customerId)
    {
        return new CartDto
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            IsCheckedOut = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CartItems = new List<CartItemDto>(),
            TotalBasePrice = 0,
            TotalDiscount = 0,
            TotalAmount = 0
        };
    }
}
