using FluentAssertions;
using SharedKernel.Interfaces;

namespace CustomerService.Tests.Application.Customers.Queries;

public class GetCustomerByIdQueryHandlerTests : IClassFixture<TestFixtureBase<GetCustomerByIdQueryHandler>>
{
    private readonly TestFixtureBase<GetCustomerByIdQueryHandler> _fixture;
    private readonly GetCustomerByIdQueryHandler _handler;
    private readonly Mock<ICacheService> _cacheServiceMock;

    public GetCustomerByIdQueryHandlerTests(TestFixtureBase<GetCustomerByIdQueryHandler> fixture)
    {
        _fixture = fixture;
        _cacheServiceMock = new Mock<ICacheService>();

        _handler = new(
            _fixture.CustomerRepository,
            _fixture.Mapper,
            _cacheServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_CustomerInCache_ReturnsCustomerDtoWithoutRepositoryCall()
    {
        // Arrange
        var cachedCustomer = _fixture.Context.Customers.First();
        var query = new GetCustomerByIdQuery(cachedCustomer.Id);

        _cacheServiceMock
            .Setup(c => c.GetAsync<Customer>(It.IsAny<string>()))
            .ReturnsAsync(cachedCustomer);

        var repositoryMock = new Mock<ICustomerRepository>();
        var handler = new GetCustomerByIdQueryHandler(repositoryMock.Object, _fixture.Mapper, _cacheServiceMock.Object);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(cachedCustomer.Id);

        repositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>(),false), Times.Never);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<Customer>(), It.IsAny<TimeSpan>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CustomerNotInCacheButExistsInRepository_ReturnsCustomerDto()
    {
        // Arrange
        var customer = _fixture.Context.Customers.First();
        var query = new GetCustomerByIdQuery(customer.Id);

        _cacheServiceMock
            .Setup(c => c.GetAsync<Customer>(It.IsAny<string>()))
            .ReturnsAsync((Customer?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(customer.Id);

        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<Customer>(), It.IsAny<TimeSpan>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CustomerDoesNotExist_ThrowsCustomerNotFoundException()
    {
        // Arrange
        var query = new GetCustomerByIdQuery(Guid.NewGuid());

        _cacheServiceMock
            .Setup(c => c.GetAsync<Customer>(It.IsAny<string>()))
            .ReturnsAsync((Customer?)null);

        // Act
        var act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<CustomerNotFoundException>();
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<Customer>(), It.IsAny<TimeSpan>()), Times.Never);
    }
}
