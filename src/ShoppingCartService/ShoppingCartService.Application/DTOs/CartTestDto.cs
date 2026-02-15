namespace ShoppingCartService.Application.DTOs;

public class CartTestDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public bool IsCheckedOut { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<CartItemTestDto> CartItems { get; set; }
}
