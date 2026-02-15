namespace ShoppingCartService.Tests.EntitiesTests;

public class CartItemTests
{
    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        var item = _createCartItem();

        item.Id.Should().Be(1);
        item.CartId.Should().Be(2);
        item.ProductId.Should().Be(10);
        item.Quantity.Value.Should().Be(2);
        item.UnitPrice.Value.Should().Be(100m);
        item.Discount.Value.Should().Be(10m);
        item.TotalPrice.Value.Should().Be(180m);
    }


    [Fact]
    public void RecalculateTotalPrice_ShouldClampToZero_WhenDiscountGreaterThanUnitPrice()
    {
        var item = new CartItem(id: 1, cartId: 2, productId: 10, quantity: 2, unitPrice: 10m, discount: 20m,
         createdAt: DateTime.Now, updatedAt: DateTime.Now);

        item.TotalPrice.Value.Should().Be(0); // (10 - 20) * 2 = -20 → clamped to 0
    }

    [Fact]
    public void UpdateQuantity_ShouldChangeQuantityAndRecalculateTotal()
    {
        var item = _createCartItem();

        item = item.UpdateQuantity(3);

        item.Quantity.Value.Should().Be(3);
        item.TotalPrice.Value.Should().Be(270); // (100 - 10) * 3
    }

    [Fact]
    public void UpdatePrice_ShouldChangePriceAndRecalculateTotal()
    {
        var item = _createCartItem();

        item = item.UpdatePrice(80m, 20m);

        item.UnitPrice.Value.Should().Be(80m);
        item.Discount.Value.Should().Be(20m);
        item.TotalPrice.Value.Should().Be(120m); // (80 - 20) * 2
    }

    private CartItem _createCartItem()
    {
        return CartItemBuilder.Empty()
                .WithId(1)
                .WithCartId(2)
                .WithProductId(10)
                .WithQuantity(2)
                .WithPrice(100)
                .WithDiscount(10m)
                .CreatedAt(DateTime.Now)
                .UpdatedAt(DateTime.Now)
                .Build();
    }
}
