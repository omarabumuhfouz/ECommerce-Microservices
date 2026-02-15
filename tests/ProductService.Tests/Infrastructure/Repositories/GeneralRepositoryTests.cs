using Microsoft.EntityFrameworkCore;

namespace ProductService.Tests.Infrastructure.Repositories;
public class GeneralRepositoryTests
{
    private readonly ProductDbContext _context;
    private readonly GeneralRepository<Category> _repository;

    public GeneralRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // each test gets a clean db
            .Options;

        _context = new ProductDbContext(options);
        _repository = new GeneralRepository<Category>(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddEntityToDatabase()
    {
        // Arrange
        var category = Category.Create(Guid.NewGuid(), "Electronics", "Electronic devices");

        // Act
        await _repository.AddAsync(category, CancellationToken.None);
        await _repository.SaveChangesAsync(CancellationToken.None);

        // Assert
        var result = await _context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
        Assert.NotNull(result);
        Assert.Equal("Electronics", result.Name);
    }

    [Fact]
    public async Task Delete_ShouldRemoveEntityFromDatabase()
    {
        // Arrange
        var category = Category.Create(Guid.NewGuid(), "Toys", "Kids toys");
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        // Act
        _repository.Delete(category);
        await _repository.SaveChangesAsync(CancellationToken.None);

        // Assert
        var result = await _context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
        Assert.Null(result);
    }

    [Fact]
    public async Task Delete_ShouldAttachDetachedEntityAndRemoveIt()
    {
        // Arrange
        var category = Category.Create(Guid.NewGuid(), "Books", "Reading books");
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        _context.Entry(category).State = EntityState.Detached; // simulate detached entity

        // Act
        _repository.Delete(category);
        await _repository.SaveChangesAsync(CancellationToken.None);

        // Assert
        var result = await _context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
        Assert.Null(result);
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldPersistChanges()
    {
        // Arrange
        var category = Category.Create(Guid.NewGuid(), "Games", "Fun games");
        await _repository.AddAsync(category, CancellationToken.None);

        // Act
        await _repository.SaveChangesAsync(CancellationToken.None);

        // Assert
        var result = await _context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
        Assert.NotNull(result);
    }
}

