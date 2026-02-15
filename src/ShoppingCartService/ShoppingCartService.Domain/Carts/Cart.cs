using SharedKernel.Primitives.Result;
using SharedKernel.Shared;
using ShoppingCartService.Domain.Errors;

namespace ShoppingCartService.Domain.CartManagement;

public record Cart : AggregateRoot
{
    public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();

    private Cart(Guid id, Guid customerId, bool isCheckedOut, DateTime createdAt, DateTime updatedAt, List<CartItem>? cartItems) : base(id)
    {
        CustomerId = customerId;
        IsCheckedOut = isCheckedOut;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        _cartItems = cartItems ?? new List<CartItem>();
    }
    
    internal Cart() { }


    public Guid CustomerId { get; private set; }
    public bool IsCheckedOut { get; private set; } = false;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    private List<CartItem> _cartItems = new List<CartItem>();


    public static Result<Cart> Create(Guid customerId,  List<CartItem> cartItems = null)
    {
        var validationResult = _Validate( customerId);
        if (validationResult.IsFailure) return validationResult.TopError;

        return new Cart(Guid.NewGuid(), customerId, false, DateTime.UtcNow, DateTime.UtcNow, cartItems);
    }

    private static Result _Validate( Guid customerId)
    {
        return Result.Success()
            .FailIf(_ValidateCustomerId(customerId), DomainErrors.Cart.CustomerIdRequired);
    }
    private static bool _ValidateId(Guid id) => id == Guid.Empty;
    private static bool _ValidateCustomerId(Guid customerId) => _ValidateId(customerId);


    public Result AddItem(Guid productId, int quantity, decimal unitPrice, decimal discount)
    {
        if (IsCheckedOut) return DomainErrors.Cart.AlreadyCheckedOut;

        if (_ValidateId(productId)) return DomainErrors.CartItem.InvalidProductId;

        if (discount > unitPrice) return DomainErrors.CartItem.DiscountExceedsPrice;


        var existingItem = GetItemByProductId(productId);
        if (existingItem.IsSuccess)
        {
            var editResult = existingItem.Value.Edit(quantity, unitPrice, discount);
            if (editResult.IsFailure) return editResult.TopError;
        }
        else
        {
            var newItemResult = CartItem.Create(
                        cartId: this.Id,
                        productId: productId,
                        quantity: quantity,
                        unitPrice: unitPrice,
                        discount: discount
                    );

            if (newItemResult.IsFailure) return newItemResult.TopError;

            _cartItems.Add(newItemResult.Value);
        }

        return Result.Success();
    }



    public Result RemoveItem(Guid cartItemId)
    {
        if (IsCheckedOut) return DomainErrors.Cart.AlreadyCheckedOut;

        var existingItemResult = GetItemByCartItemId(cartItemId);
        if (existingItemResult.IsFailure) return existingItemResult.TopError;

        _cartItems.Remove(existingItemResult.Value);

        return Result.Success();
    }

    public Result<CartItem> GetItemByCartItemId(Guid cartItemId)
    {
        var cartItem = _cartItems.FirstOrDefault(ci => ci.Id == cartItemId);
        if (cartItem is null) return DomainErrors.CartItem.NotFound(cartItemId);
        return cartItem;
    }



    public Result Checkout()
    {
        if (IsCheckedOut) return DomainErrors.Cart.AlreadyCheckedOut;

        if (!_cartItems.Any()) return DomainErrors.Cart.EmptyCart;

        IsCheckedOut = true;
        UpdatedAt = DateTime.UtcNow;


        return Result.Success();
    }

    public void Restore()
    {
        if (!IsCheckedOut) return;

        IsCheckedOut = false;
        UpdatedAt = DateTime.UtcNow;
    }

    // Change return type from CartItem? to Result<CartItem>
    public Result<CartItem> GetItemByProductId(Guid productId)
    {
        var cartItem = _cartItems.FirstOrDefault(ci => ci.ProductId == productId);

        if (cartItem is null) return DomainErrors.CartItem.ProductNotFound(productId);

        return cartItem;
    }

    public bool HasItem(Guid productId) => GetItemByProductId(productId).IsSuccess;

    public void Clear() => _cartItems.Clear();

    public decimal CalculateBasePrice() =>
        _cartItems.Sum(item => (item.UnitPrice?.Value ?? 0m) * (item.Quantity?.Value ?? 0));

    public decimal CalculateTotalDiscount() =>
        _cartItems.Sum(item => (item.Discount?.Value ?? 0m) * (item.Quantity?.Value ?? 0));

    public decimal CalculateTotalAmount() =>
        CalculateBasePrice() - CalculateTotalDiscount();

    public List<Guid> GetProductIds() => _cartItems.Select(ci => ci.ProductId).ToList();

    public Result UpdateItemQuantity(Guid cartItemId, int newQuantity)
    {
        if (IsCheckedOut) return DomainErrors.Cart.AlreadyCheckedOut;

        var existingItemResult = GetItemByCartItemId(cartItemId);

        if (existingItemResult.IsFailure) return existingItemResult.TopError;

        var item = existingItemResult.Value;

        var updateResult = item.UpdateQuantity(newQuantity);

        if (updateResult.IsFailure) return updateResult.TopError;

        return Result.Success();
    }

    public Result RemoveCartItem(Guid cartItemId)
    {
        var existingCartItemResult = GetItemByCartItemId(cartItemId);
        if (existingCartItemResult.IsFailure) return existingCartItemResult.TopError;

        _cartItems.Remove(existingCartItemResult.Value);

        return Result.Success();
    }

}