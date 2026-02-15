using Microsoft.Extensions.Logging;
using ProductService.Infrastructure.Data; 

public static class DatabaseSeeder
{
    public static async Task SeedDatabaseAsync(ProductDbContext context, ILogger logger)
    {
        // 1. Check if data already exists to prevent re-seeding
        if (await context.Categories.AnyAsync())
        {
            logger.LogInformation("Database already seeded. Skipping...");
            return;
        }

        logger.LogInformation("Starting database seeding...");

        try
        {
            // 2. Add Categories
            // These are created and tracked by the context
            var categories = SeedData.GetCategories();
            await context.Categories.AddRangeAsync(categories);
            logger.LogInformation("Added {CategoryCount} categories.", categories.Count);

            // 3. Add Tags
            // These static instances are now tracked by the context
            var tags = SeedData.GetTags();
            await context.Tags.AddRangeAsync(tags);
            logger.LogInformation("Added {TagCount} tags.", tags.Count);
            
            // 4. Add Products
            // GetProducts() links to Categories by ID (CategoryId)
            // and to Tags by object instance (Tag_Bestseller, etc.)
            // EF Core is smart enough to link them correctly.
            var products = SeedData.GetProducts();
            await context.Products.AddRangeAsync(products);
            logger.LogInformation("Added {ProductCount} products.", products.Count);

            // 5. Save all changes in a single transaction
            await context.SaveChangesAsync();
            logger.LogInformation("Database seeding completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            // Optionally, re-throw or handle as needed
            throw;
        }
    }
}