// using ProductService.Application.Mappings;

// namespace ProductService.Tests
// {
//     public static class HelperMethods
//     {
//         public static Category CreateCategory()
//         {
//             return new Category(1, "Test Category", "This is a test category", true);
//         }

//         public static List<Category> GetListOfCategory()
//         {
//             return new List<Category>
//             {
//                 new Category(1, "Category 1", "Description 1", true),
//                 new Category(2, "Category 2", "Description 2", false),
//                 new Category(3, "Category 3", "Description 3", true)
//             };
//         }

//         public static List<Product> GetListOfProducts(int categoryId = 1)
//         {
//             return new List<Product>
//                 {
//     Product.Create(
//         name: "iPhone 14",
//         description: "Apple smartphone with advanced camera system",
//         price: 999.99m,
//         categoryId: categoryId,
//         stockQuantity: 50,
//         percentage: 10,
//         image: "iphone14.jpg"
//     ),

//     Product.Create(
//         name: "Samsung Galaxy S22",
//         description: "Latest Samsung flagship phone",
//         price: 899.99m,
//         categoryId: categoryId,
//         stockQuantity: 30,
//         percentage: 5,
//         image: "galaxys22.jpg"
//     ),

//     Product.Create(
//         name: "MacBook Pro 16\"",
//         description: "Powerful laptop for professionals",
//         price: 2399.00m,
//         categoryId: categoryId,
//         stockQuantity: 20,
//         percentage: 15,
//         image: "macbookpro.jpg"
//     ),

//                 };
//         }

//         public static ProductDto CreateProductDto(Product product)
//         {
//             return new ProductDto
//             {
//                 Id = product.Id,
//                 Name = product.Name,
//                 CategoryId = product.CategoryId,
//                 Description = product.Description,
//                 ImageUrl = product.Image.Url,
//                 IsAvailable = product.IsAvailable,
//                 Price = product.Price.Amount,
//                 DiscountPercentage = product.Discount.Percentage,
//                 StockQuantity = product.StockQuantity
//             };
//         }

//         public static Product CreateProduct()
//         {
//             return Product.Create("TV", "Smart TV", 899.99m, 2, 10, 0, "tv.jpg");
//         }

//         public static List<ProductDto> GetListOfProductsDto(List<Product> products)
//         {
//             return products.Select(s => new ProductDto
//             {
//                 Id = s.Id,
//                 Name = s.Name,
//                 CategoryId = s.CategoryId,
//                 Description = s.Description,
//                 ImageUrl = s.Image.Url,
//                 IsAvailable = s.IsAvailable,
//                 Price = s.Price.Amount,
//                 DiscountPercentage = s.Discount.Percentage,
//                 StockQuantity = s.StockQuantity,
//             }).ToList();
//         }

//         public static CategoryDto CreateCategoryDto(Category category)
//         {
//             return new CategoryDto
//             {
//                 Id = category.Id,
//                 Name = category.Name,
//                 Description = category.Description,
//                 IsActive = category.IsActive
//             };
//         }

//         public static ApiResponse<T> CreateResponse<T>(T data, HttpStatusCode statusCode = HttpStatusCode.OK, bool succeeded = true, object? meta = null)
//         {
//             return new ApiResponse<T> { Data = data, Succeeded = succeeded, StatusCode = statusCode, Meta = meta };
//         }

//         //public static void SeedTestData(ProductDbContext context, IMapper mapper)
//         //{
//         //    // Load test data from your JSON file
//         //    var customers = LoadData(mapper);

//         //    // Add customers to in-memory database
//         //    context.Products.AddRange(customers);
//         //    context.SaveChanges();
//         //}

//         public static IMapper CreateMapper()
//         {
//             var mapperConfig = new MapperConfiguration(cfg =>
//             {
//                 cfg.AddProfile<ProductProfile>();
//                 cfg.AddProfile<CategoryProfile>();
//             });

//             return mapperConfig.CreateMapper();
//         }

//         public static LocalizedString GetProductAddedSuccessLocalizedMessage() =>
//              new("ProductAddedSuccessfully", "The product has been added successfully.");

//         public static LocalizedString GetCategoryNameAlreadyExistsLocalizedMessage()
//             => new("CategoryNameAlreadyExists", "A category with the same name already exists.");

//         public static LocalizedString GetNotFoundCategoryLocalizedMessage()
//             => new("CategoryNotFound", "Category not found.");

//         public static LocalizedString GetProductNameAlreadyExistsLocalizedMessage()
//             => new("ProductNameAlreadyExists", "A product with the same name already exists.");

//         public static LocalizedString GetProductNotFoundLocalizedMessage()
//             => new("ProductNotFound", "Product not found.");

//         public static LocalizedString GetProductDeletedSuccessfullyLocalizedMessage()
//             => new("ProductDeletedSuccessfully", "Product deleted successfully.");

//         public static LocalizedString GetProductUpdatedSuccessfullyLocalizedMessage()
//                   => new("ProductUpdatedSuccessfully", "Product updated successfully.");

//         public static LocalizedString GetCategoryAddedSuccessfullyLocalizedMessage()
//             => new("CategoryCreatedSuccessfully", "The category has been created successfully.");

//         public static LocalizedString GetCategoryDeletedSuccessfullyLocalizedMessage()
//             => new("CategoryDeletedSuccessfully", "The category has been deleted successfully.");

//         public static LocalizedString GetCategoryUpdatedSuccessfullyLocalizedMessage()
//             => new("CategoryUpdatedSuccessfully", "Category updated successfully.");

//         public static LocalizedString GetNoProductsForCategoryLocalizedMessage()
//             => new("NoProductsForCategory", "No products found for the specified category.");



//     }
// }
