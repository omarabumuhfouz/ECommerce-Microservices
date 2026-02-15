using SharedKernel.Primitives;
using SharedKernel.Shared;
using ShoppingCartService.Domain.CartManagement.ValueObjects;
using ShoppingCartService.Domain.Errors;

namespace ShoppingCartService.Domain.CartManagement;

public record CartItem : Entity
{

    private CartItem(
        Guid cartId, 
        Guid productId, 
        Quantity quantity, 
        Money unitPrice, 
        Money discount,
        DateTime createdAt, 
        DateTime updatedAt) : base(Guid.Empty)
    {
        CartId = cartId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = discount;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    internal CartItem() { } // For EF Core


    public Guid CartId { get; private set; }
    public Cart Cart { get; }
    public Guid ProductId { get; private set; }
    public Quantity Quantity { get; private set; } = Quantity.Zero();
    public Money UnitPrice { get; private set; } = Money.Zero();
    public Money Discount { get; private set; } = Money.Zero();
    public Money TotalPrice => RecalculateTotalPrice();
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public static Result<CartItem> Create(
            Guid cartId,
            Guid productId,
            int quantity,
            decimal unitPrice,
            decimal discount)
    {
        if (cartId == Guid.Empty) return DomainErrors.Cart.IdRequired; 
        if (productId == Guid.Empty) return DomainErrors.CartItem.InvalidProductId;

        var quantityResult = Quantity.Create(quantity);
        var unitPriceResult = Money.Create(unitPrice);
        var discountResult = Money.Create(discount);

        if (quantityResult.IsFailure) return quantityResult.TopError;
        if (unitPriceResult.IsFailure) return unitPriceResult.TopError;
        if (discountResult.IsFailure) return discountResult.TopError;

        if (discountResult.Value > unitPriceResult.Value) return DomainErrors.CartItem.DiscountExceedsPrice;

        var cartItem = new CartItem(
            cartId,
            productId,
            quantityResult.Value,
            unitPriceResult.Value,
            discountResult.Value,
            DateTime.UtcNow,
            DateTime.UtcNow
        );

        return cartItem;
    }

    public Result Edit(int quantity, decimal unitPrice, decimal discount)
    {
        var quantityResult = Quantity.Create(quantity);
        var unitPriceResult = Money.Create(unitPrice);
        var discountResult = Money.Create(discount);

        if (quantityResult.IsFailure) return quantityResult.TopError;
        if (unitPriceResult.IsFailure) return unitPriceResult.TopError;
        if (discountResult.IsFailure) return discountResult.TopError;

        if (discountResult.Value > unitPriceResult.Value) return DomainErrors.CartItem.DiscountExceedsPrice;

        Quantity = quantityResult.Value;
        UnitPrice = unitPriceResult.Value;
        Discount = discountResult.Value;
        UpdatedAt = DateTime.UtcNow;

        RecalculateTotalPrice();

        return Result.Success();
    }

    public Result UpdateQuantity(int value)
    {
        var quantityResult = Quantity.Create(value);

        if (quantityResult.IsFailure) return quantityResult.TopError;

        Quantity = quantityResult.Value;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result UpdatePrice(decimal unitPrice, decimal discount)
    {
        var priceResult = Money.Create(unitPrice);
        var discountResult = Money.Create(discount);

        if (priceResult.IsFailure) return priceResult.TopError;
        if (discountResult.IsFailure) return discountResult.TopError;

        if (discountResult.Value > priceResult.Value) return DomainErrors.CartItem.DiscountExceedsPrice;

        UnitPrice = priceResult.Value;
        Discount = discountResult.Value;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }


public Money RecalculateTotalPrice()
{
    var price = UnitPrice?.Value ?? 0m;
    var discount = Discount?.Value ?? 0m;
    var quantity = Quantity?.Value ?? 0;

    var calculated = (price - discount) * quantity;
    
    return calculated >= 0 ? Money.Create(calculated).Value : Money.Zero();
}
}
