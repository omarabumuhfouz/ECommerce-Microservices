namespace InventoryService.Domain.Errors;

public static class ErrorCodes
{
    public static class Inventory
    {
        // Validation Errors (400 Bad Request)
        public const string NegativeQuantity = "Inventory.NegativeQuantity";
        public const string ProductIdRequired = "Inventory.ProductIdRequired";

        // Conflict Errors (409 Conflict) - Logic/State issues
        public const string InsufficientStock = "Inventory.InsufficientStock";
        public const string InsufficientReservedStock = "Inventory.InsufficientReservedStock";
        public const string AlreadyExists = "Inventory.AlreadyExists";

        // Not Found Errors (404 Not Found)
        public const string NotFound = "Inventory.NotFound";
        public const string ReservationNotFound = "Inventory.ReservationNotFound";
    }
}