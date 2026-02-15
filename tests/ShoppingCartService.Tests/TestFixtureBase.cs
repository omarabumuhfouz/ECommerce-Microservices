using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ShoppingCartService.Domain.DTOs;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Settings;

namespace ProductService.Tests;

public class TestFixtureBase<T> : IDisposable
{
public WireMockServer Server { get; private set; }
public string BaseUrl => Server.Url ?? throw new InvalidOperationException("Url for Wire Mock Server is null.");
    public CartDbContext Context { get; private set; }
    public ICartRepository CartRepository { get; private set; }
    public IProductServiceClient ProductServiceClient { get; private set; }
    public ICustomerServiceClient CustomerServiceClient { get; private set; }
    public IValidationService ValidationService { get; private set; }
    public ICartMapper CartMapper { get; private set; }
    public ICacheService CacheService { get; private set; }
    public ILogger<T> Logger { get; private set; }

    public TestFixtureBase()
    {
        var options = new DbContextOptionsBuilder<CartDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        // Setup Wire Mock Server
        Server = WireMockServer.Start(new WireMockServerSettings
        {
            Urls = ["http://localhost:1333"],
            StartAdminInterface = true,
            // ReadStaticMappings = true

        });


        Context = new CartDbContext(options);
        SeedData();

        var httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };

        var productLogger = CreateLogger<ProductHttpClient>();
        var distributedOptions = Options.Create(new MemoryDistributedCacheOptions());
        IDistributedCache memoryCache = new MemoryDistributedCache(distributedOptions);

        CustomerServiceClient = new CustomerHttpClient(httpClient);
        ProductServiceClient = new ProductHttpClient(httpClient, productLogger);
        ValidationService = new ValidationService();
        CartMapper = new CartMapper(ProductServiceClient);
        CacheService = new RedisCacheService(memoryCache);
        CartRepository = new CartRepository(Context);
        Logger = CreateLogger<T>();

            }

    private void SeedData()
    {
        // var mapper = _CreateMapper();
        // // Load test data from your JSON file
        // var carts = TestDataLoader.LoadData(mapper);

        // // Add customers to in-memory database
        // Context.CartItems.AddRange(carts);
        // Context.SaveChanges();
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

    public void SetupMockProductResponse(Guid productId, string productName)
    {
        Server.Given(Request.Create()
                .WithPath($"/api/products/{productId}")
                .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyAsJson(new ProductDto
                {
                    Id = productId,
                    Name = productName,
                    Price = 100,
                    Description = "Mocked",
                    CategoryId = 1,
                    DiscountPercentage = 2,
                    ImageUrl = "",
                    IsAvailable = true,
                    StockQuantity = 100
                }));
    }

public void SetupMockProductNotFound(int productId)
    {
        Server.Given(Request.Create()
                .WithPath($"/api/products/{productId}")
                .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(404));
    }

    public void SetupMockProductNamesResponse(List<int> ids, List<string> names)
    {
        if (ids == null || names == null || ids.Count != names.Count)
            throw new ArgumentException("IDs and names must be non-null and have the same length.");

        var products = ids.Zip(names, (id, name) => new ProductNameDto(id, name)).ToList();

        var idsQuery = string.Join(",", ids);

        Server
            .Given(Request.Create()
                .WithPath($"/api/products/names")
                .UsingGet()
                .WithParam("ids", idsQuery)
            )
        .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyAsJson(products));
    }

    public void SetupMockCustomerExists(int customerId)
    {
        Server.Given(Request.Create()
                .WithPath($"/api/customers/exists/{customerId}")
                .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json"));
    }

    public void SetupMockCustomerNotExists(int customerId)
    {
        Server.Given(Request.Create()
                .WithPath($"/api/customers/exists/{customerId}")
                .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(404));

    }

    public void Dispose()
    {
        Context?.Dispose();
    }
}