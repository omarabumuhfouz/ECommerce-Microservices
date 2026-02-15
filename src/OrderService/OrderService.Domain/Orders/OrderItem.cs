using MediatR;
using OrderService.Domain.Constants;
using OrderService.Domain.Orders.ValueObjects;


namespace OrderService.Domain.Orders;

public record OrderItem : Entity
{
    private OrderItem(
        Guid id,
        Guid orderId,
        Guid productId,
        string productName,
        int quantity,
        Money unitPrice,
        decimal discountPercentage) : base(id)
    {
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        DiscountPercentage = discountPercentage;
        ProductName = productName;


        // Calculate initial totals

    }

    protected OrderItem() { }

    public Guid OrderId { get; private set; }
    public Order Order { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; } = Money.Zero;
    public decimal DiscountPercentage { get; private set; }

    // Computed Properties
    public Money Discount => Money.Create((DiscountPercentage / 100.0m) * (Quantity * UnitPrice.Value)).Value;
    
    public Money TotalPrice => Money.Create((Quantity * UnitPrice.Value) - Discount.Value).Value;

    public static Result<OrderItem> Create(
        Guid orderId,
        Guid productId,
        string productName,
        int quantity,
        decimal unitPrice,
        decimal discountPercentage)
    {
        if (productId == default) return DomainErrors.Items.InvalidProductId;

        if (quantity is < OrderItemConstants.MinQuantity) return DomainErrors.Items.InvalidQuantity;

        if (discountPercentage is < OrderItemConstants.MinPercentage or > OrderItemConstants.MaxPercentage)
            return DomainErrors.Items.InvalidDiscountPercentage;

        if(string.IsNullOrWhiteSpace(productName)) return DomainErrors.Items.InvalidProductName;



        var unitPriceResult = Money.Create(unitPrice);
        if (unitPriceResult.IsFailure) return unitPriceResult.TopError;

        return new OrderItem(
            Guid.Empty,
            orderId,
            productId,
            productName,
            quantity,
            unitPriceResult.Value,
            discountPercentage
        );
    }

    public Result<Unit> Update(int quantity, decimal unitPrice, decimal discountPercentage)
    {
        return UpdateQuantity(quantity)
        .Bind(_ => UpdatePrice(unitPrice, discountPercentage))
        .Map(_ => Unit.Value);
    }

    public Result<Unit> UpdateQuantity(int quantity)
    {
        if (quantity is < OrderItemConstants.MinQuantity) return DomainErrors.Items.InvalidQuantity;

        Quantity = quantity;

        return Unit.Value;
    }

    public Result<Unit> UpdatePrice(decimal unitPrice, decimal discountPercentage)
    {
        if (discountPercentage is < OrderItemConstants.MinPercentage or > OrderItemConstants.MaxPercentage)
            return DomainErrors.Items.InvalidDiscountPercentage;

        var priceResult = Money.Create(unitPrice);
        if (priceResult.IsFailure) return priceResult.TopError;

        UnitPrice = priceResult.Value;
        DiscountPercentage = discountPercentage;


        return Unit.Value;
    }
    
}