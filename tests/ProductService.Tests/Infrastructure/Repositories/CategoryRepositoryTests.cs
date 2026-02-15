// using Microsoft.EntityFrameworkCore;

// namespace ProductService.Tests.Infrastructure.Repositories;

// public class CategoryRepositoryTests
// {
//     private readonly ProductDbContext _context;
//     private readonly CategoryRepository _repository;

//     public CategoryRepositoryTests()
//     {
//         var options = new DbContextOptionsBuilder<ProductDbContext>()
//             .UseInMemoryDatabase(Guid.NewGuid().ToString()) // isolated test DB
//             .Options;

//         _context = new ProductDbContext(options);
//         _repository = new CategoryRepository(_context);
//     }

//     [Fact]
//     public async Task AddAsync_ShouldAddCategory()
//     {
//         // Arrange
//         var category = Category.Create(Guid.NewGuid(), "Electronics", "Devices");

//         // Act
//         await _repository.AddAsync(category, CancellationToken.None);
//         await _repository.SaveChangesAsync(CancellationToken.None);

//         // Assert
//         var result = await _context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
//         Assert.NotNull(result);
//         Assert.Equal("Electronics", result.Name);
//     }

//     [Fact]
//     public async Task GetByIdAsync_ShouldReturnCategory_WhenExists()
//     {
//         // Arrange
//         var category = Category.Create(Guid.NewGuid(), "Books", "Reading materials");
//         await _context.Categories.AddAsync(category);
//         await _context.SaveChangesAsync();

//         // Act
//         var result = await _repository.GetByIdAsync(category.Id, CancellationToken.None);

//         // Assert
//         Assert.NotNull(result);
//         Assert.Equal(category.Name, result!.Name);
//     }

//     [Fact]
//     public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
//     {
//         // Act
//         var result = await _repository.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

//         // Assert
//         Assert.Null(result);
//     }

//     [Fact]
//     public async Task IsExists_ShouldReturnTrue_WhenCategoryExists()
//     {
//         // Arrange
//         var category = Category.Create(Guid.NewGuid(), "Toys", "Kids toys");
//         await _context.Categories.AddAsync(category);
//         await _context.SaveChangesAsync();

//         // Act
//         var exists = await _repository.IsExists(category.Id, CancellationToken.None);

//         // Assert
//         Assert.True(exists);
//     }

//     [Fact]
//     public async Task IsExists_ShouldReturnFalse_WhenCategoryDoesNotExist()
//     {
//         // Act
//         var exists = await _repository.IsExists(Guid.NewGuid(), CancellationToken.None);

//         // Assert
//         Assert.False(exists);
//     }

//     [Fact]
//     public async Task HasProductsAsync_ShouldReturnTrue_WhenCategoryHasProducts()
//     {
//         // Arrange
//         var category = Category.Create(Guid.NewGuid(), "Games", "Fun stuff");
//         var product = Product.Create(
//             Guid.NewGuid(), category.Id, "Chess", "Board game", 15, "img.jpg", 5);
        
//         category = category with { Products = new[] { product }.ToList() };
//         await _context.Categories.AddAsync(category);
//         await _context.SaveChangesAsync();

//         // Act
//         var hasProducts = await _repository.HasProductsAsync(category.Id, CancellationToken.None);

//         // Assert
//         Assert.True(hasProducts);
//     }

//     [Fact]
//     public async Task HasProductsAsync_ShouldReturnFalse_WhenCategoryHasNoProducts()
//     {
//         // Arrange
//         var category = Category.Create(Guid.NewGuid(), "Empty", "No products");
//         await _context.Categories.AddAsync(category);
//         await _context.SaveChangesAsync();

//         // Act
//         var hasProducts = await _repository.HasProductsAsync(category.Id, CancellationToken.None);

//         // Assert
//         Assert.False(hasProducts);
//     }

//     [Fact]
//     public async Task IsNameExistsAsync_ShouldReturnTrue_WhenNameExists()
//     {
//         // Arrange
//         var category = Category.Create(Guid.NewGuid(), "Clothing", "Wearables");
//         await _context.Categories.AddAsync(category);
//         await _context.SaveChangesAsync();

//         // Act
//         var exists = await _repository.IsNameExistsAsync("Clothing", CancellationToken.None);

//         // Assert
//         Assert.True(exists);
//     }

//     [Fact]
//     public async Task IsNameExistsAsync_ShouldReturnFalse_WhenNameDoesNotExist()
//     {
//         // Act
//         var exists = await _repository.IsNameExistsAsync("NonExisting", CancellationToken.None);

//         // Assert
//         Assert.False(exists);
//     }

//     [Fact]
//     public async Task IsDuplicateNameAsync_ShouldReturnTrue_WhenNameExistsInOtherCategory()
//     {
//         // Arrange
//         var cat1 = Category.Create(Guid.NewGuid(), "Shoes", "Footwear");
//         var cat2 = Category.Create(Guid.NewGuid(), "Boots", "Winter footwear");
//         await _context.Categories.AddRangeAsync(cat1, cat2);
//         await _context.SaveChangesAsync();

//         // Act
//         var isDuplicate = await _repository.IsDuplicateNameAsync("Shoes", cat2.Id, CancellationToken.None);

//         // Assert
//         Assert.True(isDuplicate);
//     }

//     [Fact]
//     public async Task IsDuplicateNameAsync_ShouldReturnFalse_WhenNameUnique()
//     {
//         // Arrange
//         var cat = Category.Create(Guid.NewGuid(), "Sports", "Activities");
//         await _context.Categories.AddAsync(cat);
//         await _context.SaveChangesAsync();

//         // Act
//         var isDuplicate = await _repository.IsDuplicateNameAsync("Unique", cat.Id, CancellationToken.None);

//         // Assert
//         Assert.False(isDuplicate);
//     }

//     [Fact]
//     public void GetAllAsQueryable_ShouldReturnQueryableCategories()
//     {
//         // Arrange
//         _context.Categories.Add(Category.Create(Guid.NewGuid(), "C1", "D1"));
//         _context.Categories.Add(Category.Create(Guid.NewGuid(), "C2", "D2"));
//         _context.SaveChanges();

//         // Act
//         var query = _repository.GetAllAsQueryable();

//         // Assert
//         Assert.True(query.Any());
//         Assert.IsAssignableFrom<IQueryable<Category>>(query);
//     }
// }
