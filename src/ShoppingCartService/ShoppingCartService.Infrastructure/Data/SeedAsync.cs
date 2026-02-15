using SharedKernel.Shared;
using ShoppingCartService.Domain.CartManagement;
using ShoppingCartService.Infrastructure.Data;
// Add your DbContext namespace here
// using ShoppingCartService.Infrastructure.Persistence; 

public static class CartDataSeeder
{
    public static async Task SeedAsync(CartDbContext context)
    {
        // 1. Check if data already exists to avoid duplicates
        if (context.Carts.Any())
        {
            return; 
        }

        // 2. Create a Cart for a Customer
        var cartId = Guid.NewGuid();
        var customerId = Guid.NewGuid(); // Or a specific Guid if you are testing a specific user
        
        // Use the Factory Method to ensure invariants are met
        var cartResult = Cart.Create(
            customerId 
        );

        if (cartResult.IsFailure)
        {
            throw new Exception($"Failed to seed cart: {cartResult.TopError.Message}");
        }

        var cart = cartResult.Value;

        // 3. Add Items using the Domain Method (AddItem)
        // This automatically handles Money/Quantity Value Objects and validation
        
        // Item 1: Expensive Laptop (No Discount)
        var product1Id = Guid.Parse("11111111-1111-1111-1111-111111111111");
        cart.AddItem(product1Id, quantity: 1, unitPrice: 1500.00m, discount: 0m);

        // Item 2: Mouse (With Discount)
        var product2Id = Guid.Parse("22222222-2222-2222-2222-222222222222");
        cart.AddItem(product2Id, quantity: 2, unitPrice: 50.00m, discount: 5.00m);

        // Item 3: Keyboard
        var product3Id = Guid.Parse("33333333-3333-3333-3333-333333333333");
        cart.AddItem(product3Id, quantity: 1, unitPrice: 100.00m, discount: 0m);

        // 4. Save to Database
        context.Carts.Add(cart);
        await context.SaveChangesAsync();
    }
}