using OrderService.Domain.Constants; // Use DomainLimits & ErrorCodes
using OrderService.Domain.Orders.Enums;
using SharedKernel.Primitives.Results;

namespace OrderService.Domain.Errors;

public static class DomainErrors
{
    public static class Order
    {
        public static readonly Error CannotRemoveLastItem = Error.Conflict(
                ErrorCodes.Order.CannotRemoveLastItem,
                "An order must have at least one item. To remove all items, please cancel the order instead.");

public static Error InvalidQuantity(string productName) => Error.Validation(
                "Order.InvalidQuantity", 
                $"Quantity for '{productName}' must be greater than zero.");
        public static Error StatusNotConfigured(OrderStatus current) => Error.Validation(
            ErrorCodes.Order.StatusNotConfigured,
            $"The status '{current}' is not configured to allow transitions."
        );

        public static Error InvalidTransition(OrderStatus current, OrderStatus next) => Error.Conflict(
            ErrorCodes.Order.InvalidTransition,
            $"Cannot transition order status from '{current}' to '{next}'."
        );

        public static Error NotFound(Guid orderId) => Error.NotFound(
            ErrorCodes.Order.NotFound,
            $"Order with Id '{orderId}' was not found.");

        public static readonly Error DuplicateProduct = Error.Conflict(
            ErrorCodes.Order.DuplicateProduct,
            "Order already contains this product.");

        public static readonly Error ItemNotFound = Error.NotFound(
            ErrorCodes.Order.ItemNotFound,
            "The specified order item was not found.");

        public static readonly Error EmptyOrder = Error.Validation(
            ErrorCodes.Order.EmptyOrder,
            "Order must contain at least one item.");


        public static readonly Error BuildInvalidPrecondition = Error.Validation(
            ErrorCodes.Order.BuildInvalidPrecondition,
            "The order cannot be built because one or more preconditions have not been met.");

        public static Error CannotModifyNonPendingOrder(Guid orderId, string currentStatus) => Error.Conflict(
                ErrorCodes.Order.CannotModifyNonPendingOrder,
                $"Order '{orderId}' cannot be modified because it is currently in '{currentStatus}' status. " +
                "Modifications are only allowed when the order is 'Pending'.");

            public static Error PaymentAlreadyLinked(Guid orderId) => 
         Error.Conflict(
            ErrorCodes.Order.PaymentAlreadyLinked, 
            $"The order {orderId} is already linked to a payment and cannot be re-linked.");
    }

    public static class Items
    {
        public static readonly Error InvalidProductName = Error.Validation(
            ErrorCodes.Items.InvalidProductId, 
            "The product name cannot be empty or whitespace.");
        public static Error NotFound(Guid orderId, Guid productId) => Error.NotFound(
            ErrorCodes.Items.NotFound,
            $"Order '{orderId}' does not contain a product with ID '{productId}'. " +
            "You cannot update an item that is not already part of the order.");

        public static Error NotFound(Guid itemId) => Error.NotFound(
            ErrorCodes.Items.NotFound,
            $"Item Not found with Id {itemId}");



        public static readonly Error InvalidProductId = Error.Validation(
            ErrorCodes.Items.InvalidProductId,
            "Product Id is required.");

        public static readonly Error InvalidQuantity = Error.Validation(
            ErrorCodes.Items.InvalidQuantity,
            "Quantity must be between 1 and 100.");

        public static readonly Error InvalidDiscountPercentage = Error.Validation(
            ErrorCodes.Items.InvalidDiscountPercentage,
            "Discount percentage must be between 0 and 100.");

        public static readonly Error BuildInvalidPrecondition = Error.Validation(
                    ErrorCodes.Items.BuildInvalidPrecondition,
                    "The order Item cannot be built because one or more preconditions have not been met.");

        public static readonly Error NoneFoundToDelete =  Error.NotFound(
            ErrorCodes.Items.NoneFoundToDelete, 
            "No order item matches the specified Order and Product criteria.");

    }

    public static class Money
    {
        public static readonly Error InvalidAmount = Error.Validation(
            ErrorCodes.Money.InvalidAmount,
            "Value must be positive."
        );
    }

    public static class OrderNumber
    {
        public static readonly Error Empty = Error.Validation(
            ErrorCodes.OrderNumber.Empty,
            "Order number cannot be empty.");

        public static readonly Error TooLong = Error.Validation(
            ErrorCodes.OrderNumber.TooLong,
            $"Value cannot exceed {DomainLimits.Order.MaxNumberLength} characters."
        );
    }

    public static class Customer
    {
        public static Error AddressNotFound(Guid addressId) => Error.NotFound(
            ErrorCodes.Customer.AddressNotFound,
                $"Address with Id : {addressId}, was not found."
          );

        public static Error NotFound(Guid id) => Error.NotFound(
                           ErrorCodes.Customer.NotFound,
                           $"The customer with ID '{id}' was not found");


    }

    public static class Product
    {
        public static Error NotFound(Guid id) => Error.NotFound(
                    ErrorCodes.Product.NotFound,
                    $"The product with ID '{id}' was not found in the catalog.");

        public static Error InsufficientStock(string productName, int availableStock, int requestedQuantity) => Error.Validation(
                        ErrorCodes.Product.InsufficientStock,
                        $"Insufficient stock for '{productName}'. Available: {availableStock}, Requested: {requestedQuantity}");
    }

    public static class System
    {
        // Frontend sees "Service.Unavailable", but doesn't know WHICH service failed.
        public static readonly Error ConnectionFailed = Error.Failure(
            "System.ConnectionFailed",
            "A required internal connection could not be established. Please try again later.");

        public static readonly Error RemoteTimeout = Error.Failure(
            "System.RemoteTimeout",
            "The operation took too long to complete. Please check your connection.");

        public static readonly Error InternalError = Error.Failure(
            "System.InternalError",
            "An unexpected error occurred within our systems.");
    }

}
