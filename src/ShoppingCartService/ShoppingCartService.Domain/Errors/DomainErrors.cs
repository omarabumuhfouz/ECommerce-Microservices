using SharedKernel.Shared;
using ShoppingCartService.Domain.Constants;

namespace ShoppingCartService.Domain.Errors;

public static class DomainErrors
{
    public static class Cart
    {
        public static readonly Func<Guid, Error> NotFound = id => Error.NotFound(
            ErrorCodes.Cart.NotFound,
            $"The cart with id : {id}, was not found");

        public static readonly Func<Guid, Error> NotFoundByCustomer = customerId => Error.NotFound(
            ErrorCodes.Cart.NotFound, // Note: Previous code used NOT_FOUND here too, assuming that was intentional.
            $"The cart with related to customer: {customerId}, was not found");

        public static readonly Error IdRequired = Error.Validation(
            ErrorCodes.Cart.IdRequired,
            "Cart Id must be provided.");

        public static readonly Error CustomerIdRequired = Error.Validation(
            ErrorCodes.Cart.CustomerIdRequired,
            "Customer Id must be provided.");

        public static readonly Error AlreadyCheckedOut = Error.Conflict(
            ErrorCodes.Cart.AlreadyCheckedOut,
            "The cart has already been checked out and cannot be modified.");

        // New: Validate ProductId
        public static readonly Error InvalidProductId = Error.Validation(
            ErrorCodes.CartItem.InvalidProductId,
            "The provided Product Id is invalid."
        );

        // New: Validate Discount logic
        public static readonly Error DiscountExceedsPrice = Error.Validation(
            ErrorCodes.CartItem.DiscountExceedsPrice,
            "The discount amount cannot be greater than the unit price."
        );

        public static readonly Error EmptyCart = Error.Validation(
            ErrorCodes.Cart.EmptyCart,
            "Cannot checkout an empty cart. At least one item is required."
        );

public static Error NoCheckedOutCartToRestore(Guid customerId) => 
        Error.NotFound(
            ErrorCodes.Cart.NoCheckedOutCartToRestore, 
            $"No checked-out cart was found for customer {customerId} to perform restoration.");

    }

    public static class CartItem
    {
        public static readonly Error InvalidProductId = Error.Validation(
            ErrorCodes.CartItem.InvalidProductId,
            "The provided Product Id is invalid."
        );

        // New: Validate Discount logic
        public static readonly Error DiscountExceedsPrice = Error.Validation(
            ErrorCodes.CartItem.DiscountExceedsPrice,
            "The discount amount cannot be greater than the unit price."
        );

        public static readonly Func<Guid, Error> NotFound = id => Error.NotFound(
            ErrorCodes.CartItem.NotFound,
            $"The cart item with the id {id} Identifier was not found."
        );

        public static Func<Guid, Error> ProductNotFound = productId => Error.NotFound(
            ErrorCodes.CartItem.ProductNotFound,
            $"The product with the Id '{productId}' was not found in the cart."
        );
    }

    public static class Money
    {
        public static readonly Error InvalidAmount = Error.Validation(
            ErrorCodes.Money.InvalidAmount,
            "The amount must be greater than zero.");
    }

    public static class Product
    {
        public static readonly Error InSufficientStock = Error.Conflict(
            ErrorCodes.Product.InSufficientStock,
            "The product does not have sufficient stock for the requested quantity."
        );

        public static readonly Func<Guid, Error> NotFound = id => Error.NotFound(
            ErrorCodes.Product.NotFound,
            $"The product with id : {id}, was not found");
    }

    public static class Quantity
    {
        public static readonly Error InvalidAmount = Error.Validation(
            ErrorCodes.Quantity.InvalidAmount,
            "The amount must be greater than zero.");
    }

    public static class Customer
    {
        public static readonly Func<Guid, Error> NotFound = id => Error.NotFound(
            ErrorCodes.Customer.NotFound,
            $"The customer with id : {id}, was not found");

        public static readonly Func<Guid, Error> NotFoundByUser = id => Error.NotFound(
            ErrorCodes.Customer.NotFound,
            $"The customer with user id : {id}, was not found");
    }
}
