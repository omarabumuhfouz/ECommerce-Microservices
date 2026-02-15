namespace ProductService.Domain.Enums
{
    public enum ProductStatus
    {
        Available,       // Actively listed and ready to sell
        OutOfStock,      // Not available due to no inventory
        Discontinued,    // No longer sold permanently
        ComingSoon,      // Scheduled for future release
        Archived         // Hidden from UI, still in database
    }

}
