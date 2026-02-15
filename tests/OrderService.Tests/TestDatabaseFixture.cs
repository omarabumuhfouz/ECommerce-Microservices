using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Moq;
using OrderService.Domain.Bases;
using OrderService.Domain.DTOs;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Services;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Repositories;
using OrderService.Infrastructure.Services;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace ProductService.Tests;

public class TestDatabaseFixture<T> : IDisposable
{
    public OrderDbContext Context { get; private set; }
    public WireMockServer Server { get; private set; }
    public ApiResponseHandler ApiResponseHandler { get; private set; }
    public IOrderRepository OrderRepository { get; private set; }
    public IValidationService ValidationService { get; private set; }
    public string BaseUrl => Server.Url ?? throw new InvalidOperationException("Url for Wire Mock Server is null.");
    public IStringLocalizer<T> Localizer { get; private set; }
    public ICustomerService CustomerService { get; private set; }
    public IProductService ProductService { get; private set; }
    public IMapper Mapper { get; private set; }
    private readonly Mock<IDistributedCache> _cacheForLocalizerMock;
    public ILogger<T> Logger { get; private set;    }


    public TestDatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<OrderDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Context = new OrderDbContext(options);

        // Seed data once
        SeedData();

        Server = WireMockServer.Start(new WireMockServerSettings
        {
            Urls = ["http://localhost:1333"],
            StartAdminInterface = true,
            // ReadStaticMappings = true

        });


        ApiResponseHandler = new();
        OrderRepository = new OrderRepository(Context);
        ValidationService = new ValidationService();
        _cacheForLocalizerMock = new();

        var factory = new JsonStringLocalizerFactory(_cacheForLocalizerMock.Object);

        Localizer = new StringLocalizer<T>(factory);


        var httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };

        CustomerService = new GrpcCustomerService();
        ProductService = new GrpcProductService();
        Mapper = _CreateMapper();
    }

    private void SeedData()
    {
        var mapper = _CreateMapper();
        // Load test data from your JSON file
        var categories = TestDataLoader.LoadData(mapper);

        // Add customers to in-memory database
        Context.Orders.AddRange(categories);
        Context.SaveChanges();
    }


    private IMapper _CreateMapper()
    {
        // var mapperConfig = new MapperConfiguration(cfg =>
        // {
        //     cfg.AddProfile<OrderProfile>();
        // });

        // return mapperConfig.CreateMapper();

        return null; // Change it later
    }




    // public void SetupMockProductResponse(int productId, string productName)
    // {
    //     Server.Given(Request.Create()
    //             .WithPath($"/api/products/{productId}")
    //             .UsingGet())
    //         .RespondWith(Response.Create()
    //             .WithStatusCode(200)
    //             .WithHeader("Content-Type", "application/json")
    //             .WithBodyAsJson(new ProductDto
    //             {
    //                 Id = productId,
    //                 Name = productName,
    //                 Price = 100,
    //                 Description = "Mocked",
    //                 CategoryId = 1,
    //                 DiscountPercentage = 2,
    //                 ImageUrl = "",
    //                 IsAvailable = true,
    //                 StockQuantity = 100
    //             }));
    // }

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
        Server?.Dispose();
    }
}
