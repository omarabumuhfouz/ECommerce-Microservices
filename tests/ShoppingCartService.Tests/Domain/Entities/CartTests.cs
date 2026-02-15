namespace ShoppingCartService.Tests.EntitiesTests;

public class CartTests
{
    [Fact]
    public void Constructor_ShouldInitialize()
    {
        var cart = _createCart();

        cart.Id.Should().Be(1);
        cart.CustomerId.Should().Be(10);
        cart.IsCheckedOut.Should().BeFalse();
        cart.CartItems.Should().BeEmpty();
        cart.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void AddItem_ShouldAddNewItem_WhenProductNotExists()
    {
        var cart = _createCart();

        cart.AddItem(101, 2, 50m, 5m);

        cart.CartItems.Should().HaveCount(1);
        var item = cart.CartItems.Should().ContainSingle().Subject;
        item.ProductId.Should().Be(101);
        item.Quantity.Value.Should().Be(2);
        item.UnitPrice.Value.Should().Be(50m);
        item.Discount.Value.Should().Be(5m);
    }

    [Fact]
    public void AddItem_ShouldUpdateExistingItem_WhenProductExists()
    {
        var cart = _createCart();

        cart.AddItem(101, 1, 50m, 5m);
        cart.AddItem(101, 3, 60m, 10m);

        var item = cart.CartItems.Should().ContainSingle().Subject;
        item.Quantity.Value.Should().Be(3);
        item.UnitPrice.Value.Should().Be(60m);
        item.Discount.Value.Should().Be(10m);
    }

    [Fact]
    public void AddItem_ShouldThrowCartCheckedOutModificationException_WhenCartIsCheckedOut()
    {
        var cart = _createCart();
        cart.Checkout();

        Action act = () => cart.AddItem(1, 1, 100m, 0m);

        act.Should().Throw<CartCheckedOutModificationException>();
    }

    [Fact]
    public void RemoveItem_ShouldRemove_WhenItemExists()
    {
        var cart = _createCart();
        var itemId = cart.CartItems.First().Id;

        cart.RemoveItem(itemId);

        cart.CartItems.Should().BeEmpty();
    }

    [Fact]
    public void RemoveItem_ShouldThrowCartCheckedOutModificationException_WhenCartIsCheckedOut()
    {
        var cart = _createCart();
        cart.Checkout();

        var itemId = cart.CartItems.First().Id;

        Action act = () => cart.RemoveItem(itemId);

        act.Should().Throw<CartCheckedOutModificationException>();
    }

    [Fact]
    public void RemoveItem_ShouldThrowCartItemNotFoundException_WhenItemNotFound()
    {
        var cart = _createCart();

        Action act = () => cart.RemoveItem(999);

        act.Should().Throw<CartItemNotFoundException>();
    }

    [Fact]
    public void GetItemById_ShouldReturnItem_WhenExists()
    {
        var cart = _createCart();
        var itemId = cart.CartItems.First().Id;

        var item = cart.GetItemById(itemId);

        item.Should().NotBeNull();
        item.Id.Should().Be(itemId);
    }

    [Fact]
    public void GetItemById_ShouldThrow_WhenNotExists()
    {
        var cart = _createCart();

        Action act = () => cart.GetItemById(999);

        act.Should().Throw<CartNotFoundException>();
    }

    [Fact]
    public void Checkout_ShouldSetIsCheckedOut_WhenCartHasItems()
    {
        var cart = _createCart();

        cart.Checkout();

        cart.IsCheckedOut.Should().BeTrue();
    }

    [Fact]
    public void Checkout_ShouldThrowCartCheckedOutModificationException_WhenCartAlreadyCheckedOut()
    {
        var cart = _createCart();
        cart.Checkout();

        Action act = () => cart.Checkout();

        act.Should().Throw<CartCheckedOutModificationException>();
    }

    [Fact]
    public void Checkout_ShouldThrow_WhenCartIsEmpty()
    {
        var cart = _createCart();

        Action act = () => cart.Checkout();

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Clear_ShouldRemoveAllItems()
    {
        var cart = _createCart();

        cart.Clear();

        cart.CartItems.Should().BeEmpty();
    }

    [Fact]
    public void CalculateBasePrice_ShouldReturnSumOfUnitPricesTimesQuantity()
    {
        var cart = _createCart();
        cart.AddItem(1, 2, 100m, 10m); // 2 * 100 = 200
        cart.AddItem(2, 1, 50m, 5m);   // 1 * 50 = 50

        var result = cart.CalculateBasePrice();

        result.Should().Be(250m);
    }

    [Fact]
    public void CalculateTotalDiscount_ShouldReturnSumOfDiscountsTimesQuantity()
    {
        var cart = _createCart();
        cart.AddItem(1, 2, 100m, 10m); // 2 * 10 = 20
        cart.AddItem(2, 1, 50m, 5m);   // 1 * 5 = 5

        var result = cart.CalculateTotalDiscount();

        result.Should().Be(25m);
    }

    [Fact]
    public void CalculateTotalAmount_ShouldReturnSumOfTotalPrice()
    {
        var cart = _createCart();
        cart.AddItem(1, 2, 100m, 10m); // (100 - 10) * 2 = 180
        cart.AddItem(2, 1, 50m, 5m);   // (50 - 5) * 1 = 45

        var result = cart.CalculateTotalAmount();

        result.Should().Be(225m);
    }

    [Fact]
    public void GetProductIds_ShouldReturnAllProductIds()
    {
        var cart = _createCart();
        cart.AddItem(10, 1, 100m, 0m);
        cart.AddItem(20, 1, 100m, 0m);

        var productIds = cart.GetProductIds();

        productIds.Should().BeEquivalentTo(new[] { 10, 20, 100 });
    }

    private Cart _createCart()
    {
        return CartBuilder.Empty()
                .WithId(1)
                .WithCustomerId(10)
                .IsCheckedOut(false)
                .CreatedAt(DateTime.Now)
                .UpdatedAt(DateTime.Now)
                .WithCartItem(c => c.
                      WithId(1)
                      .WithCartId(1)
                      .WithProductId(100)
                      .WithQuantity(2)
                      .WithPrice(10)
                      .WithDiscount(2)
                      .CreatedAt(DateTime.Now)
                      .UpdatedAt(DateTime.Now)
                      )
                .Build();
    }
}
