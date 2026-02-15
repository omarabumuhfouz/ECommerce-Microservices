using FluentAssertions;

namespace CustomerService.Tests.Application.Customers.Queries;

public class CustomerExistsQueryHandlerTests : IClassFixture<TestFixtureBase<IsCustomerExistsByIdQueryHandler>>
{
    private readonly TestFixtureBase<IsCustomerExistsByIdQueryHandler> _fixture;
    private readonly IsCustomerExistsByIdQueryHandler _handler;

    public CustomerExistsQueryHandlerTests(TestFixtureBase<IsCustomerExistsByIdQueryHandler> fixture)
    {
        _fixture = fixture;

        _handler = new(
            _fixture.CustomerRepository,
        _fixture.Logger
        );
    }

    [Fact]
    public async Task Handle_CustomerExistsInDb_ReturnsTrue()
    {
        // Arrange
        var existingCustomer = _fixture.Context.Customers.First();
        var query = new IsCustomerExistsByIdQuery(existingCustomer.Id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_CustomerNotInDb_ReturnsFalse()
    {
        // Arrange
        var query = new IsCustomerExistsByIdQuery(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }
}
