using FluentAssertions;

namespace CustomerService.Tests.RepositoriesTests;

public class CustomerRepositoryTests : IClassFixture<TestFixtureBase<CustomerRepository>>
{
    private readonly TestFixtureBase<CustomerRepository> _fixture;
    private readonly CustomerRepository _repository;

    public CustomerRepositoryTests(TestFixtureBase<CustomerRepository> fixture)
    {
        _fixture = fixture;
        _repository = new CustomerRepository(_fixture.Context);
    }

    [Fact]
    public async Task AddAsync_AddsCustomerSuccessfully()
    {
        // Arrange
        var customer = Customer
        .Create(Guid.NewGuid(),Guid.NewGuid(), "Omar", "Abumuhfouz", "+962-790-123456");

        // Act
        await _repository.AddAsync(customer, CancellationToken.None);
        await _repository.SaveChangesAsync(CancellationToken.None);

        // Assert
        var dbCustomer = await _fixture.Context.Customers.FindAsync(customer.Id);
        dbCustomer.Should().NotBeNull();
        dbCustomer!.FullName.FirstName.Should().Be("Omar");
    }

    [Fact]
    public void GetAllAsQueryable_ReturnsAllCustomers()
    {
        // Arrange
        var queryable = _repository.GetAsQueryable();

        // Act & Assert
        queryable.Should().NotBeNull();
        queryable.Count().Should().Be(_fixture.Context.Customers.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCustomer_WhenExists()
    {
        // Arrange
        var customer = _fixture.Context.Customers.First();

        // Act
        var result = await _repository.GetByIdAsync(customer.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(customer.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
    {
        // Act
        var result = await _repository.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task ExistsAsync_ReturnsTrue_WhenCustomerExists()
    {
        // Arrange
        var customer = _fixture.Context.Customers.First();

        // Act
        var exists = await _repository.IsExistsByIdAsync(customer.Id, CancellationToken.None);

        // Assert
        exists.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsAsync_ReturnsFalse_WhenCustomerDoesNotExist()
    {
        // Act
        var exists = await _repository.IsExistsByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task GetByUserIdAsync_ReturnsCustomer_WhenExists()
    {
        // Arrange
        var customer = _fixture.Context.Customers.First();

        // Act
        var result = await _repository.GetByUserIdAsync(customer.UserId, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.UserId.Should().Be(customer.UserId);
    }

    [Fact]
    public async Task GetByUserIdAsync_ReturnsNull_WhenNotExists()
    {
        // Act
        var result = await _repository.GetByUserIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task ExistsByUserIdAsync_ReturnsTrue_WhenCustomerExists()
    {
        // Arrange
        var customer = _fixture.Context.Customers.First();

        // Act
        var exists = await _repository.IsExistsByUserIdAsync(customer.UserId,CancellationToken.None);

        // Assert
        exists.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsByUserIdAsync_ReturnsFalse_WhenCustomerDoesNotExist()
    {
        // Act
        var exists = await _repository.IsExistsByUserIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        exists.Should().BeFalse();
    }
}
