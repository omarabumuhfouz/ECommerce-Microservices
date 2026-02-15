using FluentAssertions;
using SharedKernel.Interfaces;

namespace CustomerService.Tests.Application.Customers.Queries;

public class GetCustomerByUserIdQueryHandlerTests : IClassFixture<TestFixtureBase<GetCustomerByUserIdQueryHandler>>
{
    private readonly TestFixtureBase<GetCustomerByUserIdQueryHandler> _fixture;
    private readonly GetCustomerByUserIdQueryHandler _handler;
    private readonly Mock<ICacheService> _cacheServiceMock;

    public GetCustomerByUserIdQueryHandlerTests(TestFixtureBase<GetCustomerByUserIdQueryHandler> fixture)
    {
        _fixture = fixture;
        _cacheServiceMock = new Mock<ICacheService>();

        _handler = new(
            _fixture.CustomerRepository,
            _cacheServiceMock.Object,
            _fixture.Mapper
        );
    }

    [Fact]
    public async Task Handle_CustomerExists_ReturnsCustomerDtoAndCachesIt()
    {
        // Arrange
        var customer = _fixture.Context.Customers.First();
        var query = new GetCustomerByUserIdQuery(customer.UserId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(customer.Id);

        _cacheServiceMock.Verify(
            c => c.SetAsync(It.Is<string>(k => k.Contains(customer.Id.ToString())),
                             It.Is<Customer>(cust => cust.Id == customer.Id),
                             It.IsAny<TimeSpan>()),
            Times.Once
        );
    }

    [Fact]
    public async Task Handle_CustomerDoesNotExist_ThrowsCustomerNotFoundByUserException()
    {
        // Arrange
        var query = new GetCustomerByUserIdQuery(Guid.NewGuid());

        // Act
        var act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<CustomerNotFoundByUserException>();
        _cacheServiceMock.Verify(
            c => c.SetAsync(It.IsAny<string>(), It.IsAny<Customer>(), It.IsAny<TimeSpan>()),
            Times.Never
        );
    }
}
