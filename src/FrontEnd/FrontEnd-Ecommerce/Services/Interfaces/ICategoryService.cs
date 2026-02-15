namespace FrontEnd_Ecommerce.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllAsync();
}

public class DummyCategoryService : ICategoryService
{
    public Task<IEnumerable<Category>> GetAllAsync()
    {
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Electronics" },
            new Category { Id = 2, Name = "Clothing" },
            new Category { Id = 3, Name = "Books" },
            new Category {Id = 4, Name = "Testing Successfully"}
        };

        return Task.FromResult<IEnumerable<Category>>(categories);
    }
}


