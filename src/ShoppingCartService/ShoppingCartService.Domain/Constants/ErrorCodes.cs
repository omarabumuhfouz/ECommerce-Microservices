namespace ShoppingCartService.Domain.Constants;

public static class ErrorCodes
{
    public static class Cart
    {
        public static readonly string NotFound = "Cart.NotFound";
        public static readonly string NotFoundByCustomer = "Cart.NotFoundByCustomer";
        public static readonly string IdRequired = "Cart.IdRequired";
        public static readonly string CustomerIdRequired = "Cart.CustomerIdRequired";
        public static readonly string AlreadyCheckedOut = "Cart.AlreadyCheckedOut";
        public static readonly string EmptyCart = "Cart.EmptyCart";
        public static readonly string UserIdRequired = "Cart.UserIdRequired";
        public static readonly string NoCheckedOutCartToRestore = "Cart.NoCheckedOutCartToRestore";
    }

    public static class CartItem
    {
        public static readonly string IdRequired = "CartItem.IdRequired";
        public static readonly string InvalidProductId = "CartItem.InvalidProductId";
        public static readonly string DiscountExceedsPrice = "CartItem.DiscountExceedsPrice";
        public static readonly string NotFound = "CartItem.NotFound";
        public static readonly string ProductNotFound = "CartItem.ProductNotFound";
    }

    public static class Money
    {
        // Usage: ErrorCodes.Money.InvalidAmount
        public static readonly string InvalidAmount = "Money.ValueMustBeGreaterThanZero";
    }

    public static class Quantity
    {
        public static readonly string InvalidAmount = "Quantity.ValueMustBeGreaterThanZero";
        public static readonly string Required = "Quantity.Required";
    }

    public static class Stock
    {
        public static readonly string InsufficientStock = "Stock.InsufficientStock";
    }

    public static class Customer
    {
        public static readonly string NotFound = "Customer.NotFound";
        public static readonly string NotFoundByUser = "Customer.NotFoundByUser";
    }

    public static class Product
    {
        public static readonly string InSufficientStock = "Product.InSufficientStock"; 
        public static readonly string NotFound = "Product.NotFound";
    }
}