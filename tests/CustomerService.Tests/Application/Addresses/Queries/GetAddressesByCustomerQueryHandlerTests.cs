using FluentAssertions;
using SharedKernel.Interfaces;

namespace CustomerService.Tests.Application.Addresses.Queries;

public class GetAddressesByCustomerQueryHandlerTests : IClassFixture<TestFixtureBase<GetAddressesByCustomerQueryHandler>>
{
    private readonly TestFixtureBase<GetAddressesByCustomerQueryHandler> _fixture;
    private readonly GetAddressesByCustomerQueryHandler _handler;
    private readonly Mock<ICacheService> _cacheServiceMock;

    public GetAddressesByCustomerQueryHandlerTests(TestFixtureBase<GetAddressesByCustomerQueryHandler> fixture)
    {
        _fixture = fixture;
        _cacheServiceMock = new Mock<ICacheService>();

        _handler = new(
            _fixture.CustomerRepository,
            _fixture.Mapper,
            _cacheServiceMock.Object,
            _fixture.Logger
        );
    }

    [Fact]
    public async Task Handle_CustomerExistsInCache_ReturnsAddresses()
    {
        // Arrange
        var customer = _fixture.Context.Customers.Include(c => c.Addresses).First();
        var query = new GetAddressesByCustomerQuery(customer.Id);

        _cacheServiceMock
            .Setup(c => c.GetAsync<Customer>(It.Is<string>(k => k.Contains(customer.Id.ToString()))))
            .ReturnsAsync(customer);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(customer.Addresses.Count);
        _cacheServiceMock.Verify(c => c.GetAsync<Customer>(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CustomerExistsInDb_ReturnsAddresses()
    {
        // Arrange
        var customer = _fixture.Context.Customers.Include(c => c.Addresses).First();
        var query = new GetAddressesByCustomerQuery(customer.Id);

        _cacheServiceMock
            .Setup(c => c.GetAsync<Customer>(It.IsAny<string>()))
            .ReturnsAsync((Customer?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(customer.Addresses.Count);
        _cacheServiceMock.Verify(c => c.GetAsync<Customer>(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CustomerDoesNotExist_ThrowsCustomerNotFoundException()
    {
        // Arrange
        var query = new GetAddressesByCustomerQuery(Guid.NewGuid());

        _cacheServiceMock
            .Setup(c => c.GetAsync<Customer>(It.IsAny<string>()))
            .ReturnsAsync((Customer?)null);

        // Act
        var act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<CustomerNotFoundException>();
    }

    [Fact]
    public async Task Handle_CustomerHasNoAddresses_ReturnsEmptyList()
    {
        // Arrange
        var customer = _fixture.Context.Customers.Include(c => c.Addresses).First();
        customer.Addresses.Clear();
        await _fixture.CustomerRepository.SaveChangesAsync(CancellationToken.None);

        var query = new GetAddressesByCustomerQuery(customer.Id);

        _cacheServiceMock
            .Setup(c => c.GetAsync<Customer>(It.IsAny<string>()))
            .ReturnsAsync((Customer?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
