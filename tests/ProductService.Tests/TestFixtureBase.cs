using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using ProductService.Application.Mappings;

namespace ProductService.Tests;

public class TestFixtureBase<T> : IDisposable
{
    public ProductDbContext Context { get; private set; }
    public IProductRepository ProductRepository { get; private set; }
    public ICategoryRepository CategoryRepository { get; private set; }
    public ITagRepository TagRepository { get; private set; }
    public ICacheService CacheService { get; private set; }
    public IMapper Mapper { get; private set; }
    private readonly Mock<IDistributedCache> _cacheForLocalizerMock;
    public ILogger<T> Logger { get; private set;    }


    public TestFixtureBase()
    {
        var options = new DbContextOptionsBuilder<ProductDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Context = new ProductDbContext(options);

        // Seed data once
        SeedData();


        ProductRepository = new ProductRepository(Context);
        CategoryRepository = new CategoryRepository(Context);
        TagRepository = new TagRepository(Context);


        Logger = CreateLogger<T>();


        // _cacheForLocalizerMock = new();

        // var factory = new JsonStringLocalizerFactory(_cacheForLocalizerMock.Object);

        // Localizer = new StringLocalizer<T>(factory);


        Mapper = _CreateMapper();
    }

    private void SeedData()
    {
        var mapper = _CreateMapper();
        // Load test data from your JSON file
        var categories = TestDataLoader.LoadData(mapper);

        // Add customers to in-memory database
        Context.Categories.AddRange(categories);
        Context.SaveChanges();
    }

private ILogger<TLogger> CreateLogger<TLogger>()
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.AddDebug();
            builder.SetMinimumLevel(LogLevel.Debug);
        });

        return loggerFactory.CreateLogger<TLogger>();
    }

    private IMapper _CreateMapper()
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductProfile>();
            cfg.AddProfile<CategoryProfile>();
        });

        return mapperConfig.CreateMapper();
    }

    public void Dispose()
    {
        Context?.Dispose();
    }

}
