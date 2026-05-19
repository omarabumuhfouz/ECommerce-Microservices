using SharedKernel.Primitives.Result;
using SharedKernel.Shared;

namespace InventoryService.Domain.Errors;

public static class DomainErrors
{
    public static class Inventory
    {
        public static Error NotFoundByProductId(Guid productId) => Error.NotFound(
            "Inventory.NotFoundByProductId",
            $"The inventory item with the product ID '{productId}' was not found.");

        public static Error ProductIdRequired => Error.Validation(
            "Inventory.ProductIdRequired",
            "Product ID is required.");

        public static Error NegativeQuantity => Error.Validation(
            "Inventory.NegativeQuantity",
            "Quantity cannot be negative.");
            
        public static Error InsufficientStock => Error.Conflict(
            "Inventory.InsufficientStock",
            "There is not enough stock to fulfill the request.");

        public static Error ReservationNotFound(Guid orderId) => Error.NotFound(
            "Inventory.ReservationNotFound",
            $"Stock reservation for order '{orderId}' was not found.");

public static Error AlreadyExists(Guid productId) => Error.Conflict(
        ErrorCodes.Inventory.AlreadyExists,
        $"Inventory record for Product '{productId}' already exists.");
    }
}
