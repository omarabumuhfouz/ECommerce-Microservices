using ProductService.Domain.Categories;
using ProductService.Domain.Products;
using ProductService.Domain.Tags;

public static class SeedData
{
    // --- Re-usable Category IDs ---
    public static readonly Guid CatId_Electronics = Guid.NewGuid();
    public static readonly Guid CatId_Clothing = Guid.NewGuid();
    public static readonly Guid CatId_Books = Guid.NewGuid();
    public static readonly Guid CatId_Home = Guid.NewGuid();
    
    // Create static Tag instances
    public static readonly Tag Tag_Bestseller = Tag.Create("Bestseller").Value;
    public static readonly Tag Tag_New = Tag.Create("New").Value;
    public static readonly Tag Tag_Sale = Tag.Create("Sale").Value;
    public static readonly Tag Tag_EcoFriendly = Tag.Create("Eco-Friendly").Value;
    public static readonly Tag Tag_Smart = Tag.Create("Smart").Value;
    public static readonly Tag Tag_4K = Tag.Create("4K").Value;
    public static readonly Tag Tag_Cotton = Tag.Create("Cotton").Value;
    public static readonly Tag Tag_Wireless = Tag.Create("Wireless").Value;
    public static readonly Tag Tag_Premium = Tag.Create("Premium").Value;

    public static List<Category> GetCategories()
    {
        return new List<Category>
        {
            Category.Create(CatId_Electronics, "Electronics", "Gadgets, computers, and entertainment systems.").Value,
            Category.Create(CatId_Clothing, "Clothing", "Apparel for all ages.").Value,
            Category.Create(CatId_Books, "Books", "Hardcover, paperback, and e-books.").Value,
            Category.Create(CatId_Home, "Home & Kitchen", "Furniture, decor, and kitchenware.").Value
        };
    }

    public static List<Tag> GetTags()
    {
        return new List<Tag>
        {
            Tag_Bestseller, Tag_New, Tag_Sale, Tag_EcoFriendly, 
            Tag_Smart, Tag_4K, Tag_Cotton, Tag_Wireless, Tag_Premium
        };
    }

    // --- Main Product Seeder with Real Images ---
    public static List<Product> GetProducts()
    {
        var products = new List<Product>();

        // --- Electronics (5) ---
        products.Add(ProductBuilder.CreateNew()
            .WithId(Guid.NewGuid())
            .WithName("Samsung 65\" 4K UHD Smart TV")
            .WithDescription("Crystal Processor 4K: See every detail with stunning clarity. Smart TV with Netflix, YouTube, Disney+ and more.")
            .WithPrice(699.99m)
            .WithStockQuantity(50)
            .WithCategoryId(CatId_Electronics)
            .WithMainImage("https://images.unsplash.com/photo-1593359677879-a4bb92f829d1?w=500&h=500&fit=crop", "Samsung 65 inch 4K Smart TV")
            .AddRelatedImage("https://images.unsplash.com/photo-1593359677879-a4bb92f829d1?w=500&h=500&fit=crop")
            .AddRelatedImage("https://images.unsplash.com/photo-1571415060716-baff5f1c10ba?w=500&h=500&fit=crop")
            .AddFeature("Screen Size", "65-inch")
            .AddFeature("Resolution", "3840 x 2160 4K UHD")
            .AddFeature("Smart Platform", "Tizen OS")
            .AddFeature("HDR", "HDR10+")
            .AddTag(Tag_4K)
            .AddTag(Tag_Smart)
            .AddTag(Tag_Bestseller)
            .Build()
        );
        
        products.Add(ProductBuilder.CreateNew()
            .WithName("Sony WH-1000XM4 Wireless Headphones")
            .WithDescription("Industry-leading noise cancellation with Dual Noise Sensor technology. 30-hour battery life with quick charging.")
            .WithPrice(349.99m)
            .WithDiscount(15, DateTime.UtcNow.AddDays(30))
            .WithStockQuantity(75)
            .WithCategoryId(CatId_Electronics)
            .WithMainImage("https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=500&h=500&fit=crop", "Black wireless headphones with noise cancellation")
            .AddRelatedImage("https://images.unsplash.com/photo-1484704849700-f032a568e944?w=500&h=500&fit=crop")
            .AddFeature("Noise Cancellation", "Industry-leading ANC")
            .AddFeature("Battery Life", "30 hours")
            .AddFeature("Connectivity", "Bluetooth 5.0")
            .AddFeature("Quick Charge", "10 min = 5 hours")
            .AddTag(Tag_Wireless)
            .AddTag(Tag_Premium)
            .AddTag(Tag_Sale)
            .Build()
        );

        products.Add(ProductBuilder.CreateNew()
            .WithName("MacBook Pro 14\" M3")
            .WithDescription("Supercharged by M3 chip. 8-core CPU, 10-core GPU, 16-core Neural Engine. 16GB unified memory, 512GB SSD.")
            .WithPrice(1999.00m)
            .WithStockQuantity(25)
            .WithCategoryId(CatId_Electronics)
            .WithMainImage("https://images.unsplash.com/photo-1517336714731-489689fd1ca8?w=500&h=500&fit=crop", "MacBook Pro laptop on desk")
            .AddRelatedImage("https://images.unsplash.com/photo-1496181133206-80ce9b88a853?w=500&h=500&fit=crop")
            .AddFeature("Processor", "Apple M3 chip")
            .AddFeature("Memory", "16GB Unified Memory")
            .AddFeature("Storage", "512GB SSD")
            .AddFeature("Display", "14.2-inch Liquid Retina XDR")
            .AddTag(Tag_Premium)
            .AddTag(Tag_Bestseller)
            .Build()
        );

        products.Add(ProductBuilder.CreateNew()
            .WithName("Apple iPhone 15 Pro")
            .WithDescription("Titanium. So strong. So light. So Pro. A17 Pro chip. Action button. 48MP Main camera.")
            .WithPrice(999.00m)
            .WithStockQuantity(100)
            .WithCategoryId(CatId_Electronics)
            .WithMainImage("https://images.unsplash.com/photo-1592750475338-74b7b21085ab?w=500&h=500&fit=crop", "iPhone 15 Pro in natural titanium")
            .AddRelatedImage("https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?w=500&h=500&fit=crop")
            .AddFeature("Chip", "A17 Pro")
            .AddFeature("Camera", "48MP Main + 12MP Ultra Wide")
            .AddFeature("Storage", "128GB")
            .AddFeature("Material", "Titanium")
            .AddTag(Tag_New)
            .AddTag(Tag_Premium)
            .Build()
        );

        products.Add(ProductBuilder.CreateNew()
            .WithName("DJI Mini 3 Pro Drone")
            .WithDescription("Ultra-lightweight camera drone with 4K HDR video, 48MP photos, and 34-minute flight time.")
            .WithPrice(759.00m)
            .WithStockQuantity(40)
            .WithCategoryId(CatId_Electronics)
            .WithMainImage("https://images.unsplash.com/photo-1473968512647-3e447244af8f?w=500&h=500&fit=crop", "DJI drone flying in the air")
            .AddRelatedImage("https://images.unsplash.com/photo-1507582020474-9a35b7d455d9?w=500&h=500&fit=crop")
            .AddFeature("Camera", "4K HDR Video")
            .AddFeature("Weight", "Under 249g")
            .AddFeature("Flight Time", "34 minutes")
            .AddFeature("Obstacle Sensing", "Tri-directional")
            .AddTag(Tag_New)
            .AddTag(Tag_Smart)
            .Build()
        );

        // --- Clothing (5) ---
        products.Add(ProductBuilder.CreateNew()
            .WithName("Nike Air Force 1 '07")
            .WithDescription("The radiance lives on in the Nike Air Force 1 '07. Crossing hardwood comfort with off-court flair.")
            .WithPrice(110.00m)
            .WithStockQuantity(200)
            .WithCategoryId(CatId_Clothing)
            .WithMainImage("https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=500&h=500&fit=crop", "White Nike Air Force 1 shoes")
            .AddRelatedImage("https://images.unsplash.com/photo-1606107557195-0e29a4b5b4aa?w=500&h=500&fit=crop")
            .AddFeature("Material", "Leather and Rubber")
            .AddFeature("Color", "White/White")
            .AddFeature("Style", "Low-top")
            .AddTag(Tag_Bestseller)
            .Build()
        );

        products.Add(ProductBuilder.CreateNew()
            .WithName("Levi's 511 Slim Jeans")
            .WithDescription("Slim fit jeans that sit below the waist. Slim through hip and thigh with a narrow leg opening.")
            .WithPrice(79.50m)
            .WithDiscount(20)
            .WithStockQuantity(150)
            .WithCategoryId(CatId_Clothing)
            .WithMainImage("https://images.unsplash.com/photo-1542272604-787c3835535d?w=500&h=500&fit=crop", "Blue denim jeans laid flat")
            .AddRelatedImage("https://images.unsplash.com/photo-1582418702059-97ebafb35d09?w=500&h=500&fit=crop")
            .AddFeature("Fit", "Slim")
            .AddFeature("Material", "99% Cotton, 1% Elastane")
            .AddFeature("Wash", "Dark Stonewash")
            .AddTag(Tag_Sale)
            .AddTag(Tag_Cotton)
            .Build()
        );

        products.Add(ProductBuilder.CreateNew()
            .WithName("The North Face ThermoBall Eco Jacket")
            .WithDescription("Made with recycled materials, this jacket provides warmth even when wet and packs into its own pocket.")
            .WithPrice(149.00m)
            .WithStockQuantity(80)
            .WithCategoryId(CatId_Clothing)
            .WithMainImage("https://images.unsplash.com/photo-1551028719-00167b16eac5?w=500&h=500&fit=crop", "Green insulated jacket")
            .AddRelatedImage("https://images.unsplash.com/photo-1591047139829-d91aecb6caea?w=500&h=500&fit=crop")
            .AddFeature("Insulation", "ThermoBall Eco")
            .AddFeature("Material", "100% Recycled Shell")
            .AddFeature("Weight", "Lightweight")
            .AddFeature("Packability", "Packs into pocket")
            .AddTag(Tag_EcoFriendly)
            .AddTag(Tag_New)
            .Build()
        );

        products.Add(ProductBuilder.CreateNew()
            .WithName("Patagonia Better Sweater Fleece Jacket")
            .WithDescription("A customer favorite, the Better Sweater is a versatile, casual fleece with the feel and look of a sweater.")
            .WithPrice(139.00m)
            .WithStockQuantity(60)
            .WithCategoryId(CatId_Clothing)
            .WithMainImage("https://images.unsplash.com/photo-1434389677669-e08b4cac3105?w=500&h=500&fit=crop", "Gray fleece jacket")
            .AddRelatedImage("https://images.unsplash.com/photo-1508214751196-bcfd4ca60f91?w=500&h=500&fit=crop")
            .AddFeature("Material", "100% Recycled Polyester")
            .AddFeature("Style", "Full-zip")
            .AddFeature("Pockets", "2 handwarmer pockets")
            .AddTag(Tag_EcoFriendly)
            .Build()
        );

        products.Add(ProductBuilder.CreateNew()
            .WithName("Adidas Ultraboost 22 Running Shoes")
            .WithDescription("Ultraboost 22 shoes combine responsive cushioning and a flexible fit for energized running.")
            .WithPrice(180.00m)
            .WithStockQuantity(120)
            .WithCategoryId(CatId_Clothing)
            .WithMainImage("https://images.unsplash.com/photo-1606107557195-0e29a4b5b4aa?w=500&h=500&fit=crop", "Adidas Ultraboost running shoes")
            .AddRelatedImage("https://images.unsplash.com/photo-1549298916-b41d501d3772?w=500&h=500&fit=crop")
            .AddFeature("Cushioning", "Boost midsole")
            .AddFeature("Upper", "Primeblue material")
            .AddFeature("Use", "Road Running")
            .AddTag(Tag_New)
            .Build()
        );

        // --- Books (5) ---
        products.Add(ProductBuilder.CreateNew()
            .WithName("Atomic Habits by James Clear")
            .WithDescription("Tiny Changes, Remarkable Results: An Easy & Proven Way to Build Good Habits & Break Bad Ones")
            .WithPrice(27.00m)
            .WithStockQuantity(300)
            .WithCategoryId(CatId_Books)
            .WithMainImage("https://images.unsplash.com/photo-1544947950-fa07a98d237f?w=500&h=500&fit=crop", "Atomic Habits book cover")
            .AddRelatedImage("https://images.unsplash.com/photo-1481627834876-b7833e8f5570?w=500&h=500&fit=crop")
            .AddFeature("Author", "James Clear")
            .AddFeature("Pages", "320")
            .AddFeature("Format", "Hardcover")
            .AddTag(Tag_Bestseller)
            .Build()
        );

        products.Add(ProductBuilder.CreateNew()
            .WithName("The Midnight Library by Matt Haig")
            .WithDescription("Between life and death there is a library, and within that library, the shelves go on forever.")
            .WithPrice(26.00m)
            .WithStockQuantity(180)
            .WithCategoryId(CatId_Books)
            .WithMainImage("https://images.unsplash.com/photo-1629992101753-56d196c8aabb?w=500&h=500&fit=crop", "The Midnight Library book cover")
            .AddRelatedImage("https://images.unsplash.com/photo-1512820790803-83ca734da794?w=500&h=500&fit=crop")
            .AddFeature("Author", "Matt Haig")
            .AddFeature("Pages", "304")
            .AddFeature("Genre", "Fiction")
            .AddTag(Tag_New)
            .Build()
        );

        products.Add(ProductBuilder.CreateNew()
            .WithName("Dune by Frank Herbert")
            .WithDescription("Set in the distant future amidst a feudal interstellar society, Dune tells the story of young Paul Atreides.")
            .WithPrice(17.99m)
            .WithDiscount(10)
            .WithStockQuantity(220)
            .WithCategoryId(CatId_Books)
            .WithMainImage("https://images.unsplash.com/photo-1589998059171-988d887df646?w=500&h=500&fit=crop", "Dune book cover")
            .AddRelatedImage("https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=500&h=500&fit=crop")
            .AddFeature("Author", "Frank Herbert")
            .AddFeature("Pages", "688")
            .AddFeature("Genre", "Science Fiction")
            .AddTag(Tag_Sale)
            .Build()
        );

        products.Add(ProductBuilder.CreateNew()
            .WithName("The Psychology of Money by Morgan Housel")
            .WithDescription("Timeless lessons on wealth, greed, and happiness. Doing well with money isn't necessarily what you know, but how you behave.")
            .WithPrice(23.99m)
            .WithStockQuantity(190)
            .WithCategoryId(CatId_Books)
            .WithMainImage("https://images.unsplash.com/photo-1588666309990-d68f08e3d4a6?w=500&h=500&fit=crop", "Psychology of Money book cover")
            .AddRelatedImage("https://images.unsplash.com/photo-1544716278-ca5e3f4abd8c?w=500&h=500&fit=crop")
            .AddFeature("Author", "Morgan Housel")
            .AddFeature("Pages", "256")
            .AddFeature("Genre", "Personal Finance")
            .AddTag(Tag_Bestseller)
            .Build()
        );

        products.Add(ProductBuilder.CreateNew()
            .WithName("Project Hail Mary by Andy Weir")
            .WithDescription("A lone astronaut must save the earth from disaster in this incredible new science-based thriller from the #1 New York Times bestselling author.")
            .WithPrice(28.99m)
            .WithStockQuantity(140)
            .WithCategoryId(CatId_Books)
            .WithMainImage("https://images.unsplash.com/photo-1532012197227-501767a6c8bb?w=500&h=500&fit=crop", "Project Hail Mary book cover")
            .AddRelatedImage("https://images.unsplash.com/photo-1516979187457-637abb4f9353?w=500&h=500&fit=crop")
            .AddFeature("Author", "Andy Weir")
            .AddFeature("Pages", "496")
            .AddFeature("Genre", "Science Fiction")
            .AddTag(Tag_New)
            .Build()
        );
        
        // --- Home & Kitchen (5) ---
        products.Add(ProductBuilder.CreateNew()
            .WithName("Breville Barista Express Espresso Machine")
            .WithDescription("The Breville Barista Express delivers third wave specialty coffee at home using the 4 keys formula and is part of the Barista Series.")
            .WithPrice(699.95m)
            .WithStockQuantity(35)
            .WithCategoryId(CatId_Home)
            .WithMainImage("https://images.unsplash.com/photo-1495474472287-4d71bcdd2085?w=500&h=500&fit=crop", "Breville espresso machine making coffee")
            .AddRelatedImage("https://images.unsplash.com/photo-1561047029-3000c68339ca?w=500&h=500&fit=crop")
            .AddFeature("Grinder", "Integrated conical burr grinder")
            .AddFeature("Pressure", "15 bar Italian pump")
            .AddFeature("Water Tank", "2L removable")
            .AddTag(Tag_Premium)
            .AddTag(Tag_Bestseller)
            .Build()
        );

        products.Add(ProductBuilder.CreateNew()
            .WithName("Ninja Foodi 8-in-1 Digital Air Fry Oven")
            .WithDescription("The Ninja Foodi Digital Air Fry Oven bakes, toasts, air fries, and more with 8 cooking functions in a compact countertop design.")
            .WithPrice(229.99m)
            .WithStockQuantity(90)
            .WithCategoryId(CatId_Home)
            .WithMainImage("https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?w=500&h=500&fit=crop", "Ninja Foodi air fryer oven")
            .AddRelatedImage("https://images.unsplash.com/photo-1570222094114-d054a817e56b?w=500&h=500&fit=crop")
            .AddFeature("Functions", "8 cooking functions")
            .AddFeature("Capacity", "Fits 6 slices of bread")
            .AddFeature("Technology", "Cyclonic Air Fry Technology")
            .AddTag(Tag_Smart)
            .Build()
        );

        products.Add(ProductBuilder.CreateNew()
            .WithName("Roomba j7+ Self-Emptying Robot Vacuum")
            .WithDescription("The Roomba j7+ robot vacuum identifies and avoids obstacles like cords, socks, and pet waste. Empites itself for 60 days.")
            .WithPrice(849.00m)
            .WithDiscount(12)
            .WithStockQuantity(45)
            .WithCategoryId(CatId_Home)
            .WithMainImage("https://images.unsplash.com/photo-1558618047-3c8c76ca7d13?w=500&h=500&fit=crop", "Roomba robot vacuum cleaning floor")
            .AddRelatedImage("https://images.unsplash.com/photo-1586953208448-b95a79798f07?w=500&h=500&fit=crop")
            .AddFeature("Navigation", "PrecisionVision Navigation")
            .AddFeature("Self-Emptying", "60-day capacity")
            .AddFeature("Smart Mapping", "Imprint Smart Mapping")
            .AddTag(Tag_Smart)
            .AddTag(Tag_Sale)
            .Build()
        );

        products.Add(ProductBuilder.CreateNew()
            .WithName("Wüsthof Classic 8-Inch Chef's Knife")
            .WithDescription("Precision-forged from a single piece of high-carbon stainless steel. Features a full tang and triple-riveted handle.")
            .WithPrice(179.95m)
            .WithStockQuantity(85)
            .WithCategoryId(CatId_Home)
            .WithMainImage("https://images.unsplash.com/photo-1594736797933-d0d69c3b9d58?w=500&h=500&fit=crop", "Wusthof chef knife on cutting board")
            .AddRelatedImage("https://images.unsplash.com/photo-1583778176476-4a8b9f0d19bc?w=500&h=500&fit=crop")
            .AddFeature("Blade Length", "8 inches")
            .AddFeature("Material", "High-carbon stainless steel")
            .AddFeature("Handle", "Triple-riveted")
            .AddTag(Tag_Premium)
            .Build()
        );

        products.Add(ProductBuilder.CreateNew()
            .WithName("Vitamix 5200 Blender")
            .WithDescription("Professional-grade blender with 64-ounce container, 2.2 peak horsepower motor, and variable speed control.")
            .WithPrice(449.95m)
            .WithStockQuantity(55)
            .WithCategoryId(CatId_Home)
            .WithMainImage("https://images.unsplash.com/photo-1571330735066-03aaa9429d89?w=500&h=500&fit=crop", "Vitamix blender on counter")
            .AddRelatedImage("https://images.unsplash.com/photo-1567401893410-9b7f64a1fa67?w=500&h=500&fit=crop")
            .AddFeature("Motor", "2.2 peak HP")
            .AddFeature("Container", "64 oz low-profile")
            .AddFeature("Warranty", "7-year full")
            .AddTag(Tag_Premium)
            .AddTag(Tag_Bestseller)
            .Build()
        );

        return products;
    }
}