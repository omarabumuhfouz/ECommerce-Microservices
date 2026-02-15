using FluentAssertions;
using Microsoft.VisualBasic;

namespace CustomerService.Tests.Application.Customers.Queries;

public class GetAllCustomersQueryHandlerTests : IClassFixture<TestFixtureBase<GetAllCustomersQueryHandler>>
{
    private readonly TestFixtureBase<GetAllCustomersQueryHandler> _fixture;
    private readonly GetAllCustomersQueryHandler _handler;

    public GetAllCustomersQueryHandlerTests(TestFixtureBase<GetAllCustomersQueryHandler> fixture)
    {
        _fixture = fixture;
        _handler = new(
            _fixture.CustomerRepository,
            fixture.Mapper
        );
    }

    [Fact]
    public async Task Handle_WhenCalled_ReturnsAllCustomersAsDto()
    {
        // Arrange
        var query = new GetAllCustomersQuery();

        var expectedCount = _fixture.Context.Customers.Count();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(expectedCount);

        // check that mapping works
        var firstCustomer = _fixture.Context.Customers.First();
        var firstDto = result.FirstOrDefault(r => r.Id == firstCustomer.Id);
        firstDto.Should().NotBeNull();
        firstDto.FirstName.Should().Be(firstCustomer.FullName.FirstName);
        firstDto.PhoneNumber.Should().Be(firstCustomer.PhoneNumber.Value);
    }

    [Fact]
    public async Task Handle_WhenNoCustomers_ReturnsEmptyList()
    {
        // Arrange
        // Clear the in-memory DB
        _fixture.Context.Customers.RemoveRange(_fixture.Context.Customers);
        _fixture.Context.SaveChanges();

        var query = new GetAllCustomersQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
