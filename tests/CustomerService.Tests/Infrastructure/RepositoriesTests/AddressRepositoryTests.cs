using FluentAssertions;

namespace CustomerService.Tests.RepositoriesTests;

public class AddressRepositoryTests : IClassFixture<TestFixtureBase<AddressRepository>>
{
    private readonly TestFixtureBase<AddressRepository> _fixture;
    private readonly AddressRepository _repository;

    public AddressRepositoryTests(TestFixtureBase<AddressRepository> fixture)
    {
        _fixture = fixture;
        _repository = new AddressRepository(_fixture.Context);
    }

    [Fact]
    public async Task AddAsync_AddsAddressSuccessfully()
    {
        // Arrange
        var customer = _fixture.Context.Customers.First();
        var address = _CreateAddress(customer.Id);

        // Act
        await _repository.AddAsync(address, CancellationToken.None);
        await _repository.SaveChangesAsync(CancellationToken.None);

        // Assert
        var dbAddress = await _fixture.Context.Addresses.FindAsync(address.Id);
        dbAddress.Should().NotBeNull();
        dbAddress!.AddressLine1.Should().Be(address.AddressLine1);
    }

    [Fact]
    public async Task Delete_RemovesAddressSuccessfully()
    {
        // Arrange
        var customer = _fixture.Context.Customers.First();
        var address = _CreateAddress(customer.Id);

        await _repository.AddAsync(address, CancellationToken.None);
        await _repository.SaveChangesAsync(CancellationToken.None);

        // Act
        _repository.Delete(address);
        await _repository.SaveChangesAsync(CancellationToken.None);

        // Assert
        var dbAddress = await _fixture.Context.Addresses.FindAsync(address.Id);
        dbAddress.Should().BeNull();
    }

    [Fact]
    public async Task GetAddressesByCustomerId_ReturnsAddresses()
    {
        // Arrange
        var customer = _fixture.Context.Customers.First();
        var address = _CreateAddress(customer.Id);

        await _repository.AddAsync(address, CancellationToken.None);
        await _repository.SaveChangesAsync(CancellationToken.None);

        // Act
        var addresses = await _repository.GetAddressesByCustomerId(customer.Id, CancellationToken.None);

        // Assert
        addresses.Should().ContainSingle(a => a.Id == address.Id);
    }

    [Fact]
    public async Task GetByCustomerAndAddressId_ReturnsAddress_WhenExists()
    {
        // Arrange
        var customer = _fixture.Context.Customers.First();
        var address = _CreateAddress(customer.Id);

        await _repository.AddAsync(address, CancellationToken.None);
        await _repository.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await _repository.GetByCustomerAndAddressId(address.Id, customer.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(address.Id);
    }

    [Fact]
    public async Task GetByCustomerAndAddressId_ReturnsNull_WhenNotExists()
    {
        // Act
        var result = await _repository.GetByCustomerAndAddressId(Guid.NewGuid(), Guid.NewGuid(), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsAddress_WhenExists()
    {
        // Arrange
        var customer = _fixture.Context.Customers.First();
        var address = _CreateAddress(customer.Id);

        await _repository.AddAsync(address, CancellationToken.None);
        await _repository.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await _repository.GetByIdAsync(address.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(address.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
    {
        // Act
        var result = await _repository.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    private Address _CreateAddress(Guid customerId)
            => Address.Create
        (
            Guid.NewGuid(),
            customerId,
            "Line1",
            "Line2",
            "Amman",
            "Amman",
            "11111",
            "Jordan",
            true
        );
}
