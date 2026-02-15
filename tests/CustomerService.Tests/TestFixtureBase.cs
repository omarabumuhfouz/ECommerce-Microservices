using CustomerService.Domain;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using SharedKernel.Interfaces;
using SharedKernel.Bases;
namespace CustomerService.Tests;

public class TestFixtureBase<T> : IDisposable
{
    private readonly DbContextOptions<CustomerDbContext> _options;
    public CustomerDbContext Context { get; private set; }
    public ApiResponseHandler ApiResponseHandler { get; private set; }
    public ICustomerRepository CustomerRepository { get; private set; }
    public IAddressRepository AddressRepository { get; private set; }
    public ICacheService CacheService { get; private set; }
    public IMapper Mapper { get; private set; }
    private readonly Mock<IDistributedCache> _cacheForLocalizerMock;
    public ILogger<T> Logger { get; private set; }

    public TestFixtureBase()
    {
        _options = new DbContextOptionsBuilder<CustomerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Mapper = _CreateMapper(); 

        SeedData();

        Context = new CustomerDbContext(_options);

        ApiResponseHandler = new();
        CustomerRepository = new CustomerRepository(Context);
        AddressRepository = new AddressRepository(Context);

        _cacheForLocalizerMock = new();
        Logger = CreateLogger<T>();
    }

    private void SeedData()
    {
        using (var seedContext = new CustomerDbContext(_options))
        {
            var categories = TestDataLoader.LoadData(Mapper);

            seedContext.Customers.AddRange(categories);
            seedContext.SaveChanges();
        }
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
            cfg.AddProfile<CustomerProfile>();
        });
        return mapperConfig.CreateMapper();
    }

    public void Dispose()
    {
        Context?.Dispose();
    }
}