namespace OrderService.Domain.Constants;

public static class ErrorCodes
{
    public static class Order
    {
        public static readonly string NotFound = "Order.NotFound";
        public static readonly string InvalidQuantity = "Order.InvalidQuantity";
        public static readonly string IdRequired = "Order.IdRequired";
        public static readonly string StatusNotConfigured = "Order.StatusNotConfigured";
        public static readonly string InvalidTransition = "Order.InvalidTransition";
        public static readonly string DuplicateProduct = "Order.DuplicateProduct";
        public static readonly string ItemNotFound = "Order.ItemNotFound";
        public static readonly string EmptyOrder = "Order.EmptyOrder";
        public static readonly string BuildInvalidPrecondition = "Order.BuildInvalidPrecondition";
        public static readonly string CannotModifyNonPendingOrder = "Order.CannotModifyNonPendingOrder";
        public static readonly string CannotRemoveLastItem = "Order.CannotRemoveLastItem";
        public static readonly string PaymentAlreadyLinked = "Order.PaymentAlreadyLinked";
    }

    public static class Items
    {
        public static readonly string IdRequired = "Items.IdRequired";
        public static readonly string InvalidProductId = "Items.InvalidProductId";
        public static readonly string InvalidQuantity = "Items.InvalidQuantity";
        public static readonly string InvalidDiscountPercentage = "Items.InvalidDiscountPercentage";
        public static readonly string BuildInvalidPrecondition = "Items.BuildInvalidPrecondition";
        public static readonly string NotFound = "Items.NotFound";
        public static readonly string NoneFoundToDelete = "Items.NoneFoundToDelete";
        public static readonly string InvalidProductName = "Items.InvalidProductName";
    }

    public static class Money
    {
        public static readonly string InvalidAmount = "Money.MustBePositive";
    }

    public static class OrderNumber
    {
        public static readonly string Empty = "OrderNumber.Empty";
        public static readonly string TooLong = "OrderNumber.TooLong";
    }

    public static class Customer
    {
        public static readonly string AddressNotFound = "Customer.AddressNotFound";
        public static readonly string UserIdRequired = "Customer.UserIdRequired";
        public static readonly string IdRequired = "Customer.IdRequired";
        public static readonly string NotFound = "Customer.NotFound";
    }

    public static class Product
    {
        public static readonly string NotFound = "Product.NotFound";
        public static readonly string IdRequired = "Product.IdRequired";
        public static readonly string InsufficientStock = "Product.InsufficientStock";
    }

    public static class System
    {
        public static readonly string ConnectionFailed = "System.ConnectionFailed";
        public static readonly string RemoteTimeout = "System.RemoteTimeout";
        public static readonly string InternalError = "System.InternalError";
    }

    public static class Payment
    {
        public static readonly string IdRequired = "payment.IdRequired";
    }
    
}