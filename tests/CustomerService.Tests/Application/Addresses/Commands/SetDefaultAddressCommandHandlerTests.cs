using CustomerService.Domain;
using Microsoft.Extensions.Logging;
using FluentAssertions;
namespace CustomerService.Tests.Application.Addresses.Commands;

public class SetDefaultAddressCommandHandlerTests : IClassFixture<TestFixtureBase<SetDefaultAddressCommandHandler>>
{
    private readonly TestFixtureBase<SetDefaultAddressCommandHandler> _fixture;
    private readonly SetDefaultAddressCommandHandler _handler;

    public SetDefaultAddressCommandHandlerTests(TestFixtureBase<SetDefaultAddressCommandHandler> fixture)
    {
        _fixture = fixture;
        _handler = new(
            _fixture.CustomerRepository,
            _fixture.Logger,
            _fixture.AddressRepository
        );
    }


    [Fact]
    public async Task Handle_ValidRequest_SetsDefaultAddressSuccessfully()
    {
        // Arrange
        //  Define IDs and data from JSON file.
        var customerId = Guid.Parse("f5b2d9f4-5a84-4d83-8f44-ef2e8a6d8b01");
        var initialDefaultId = Guid.Parse("9a1f2b30-0a42-4de1-bdfc-b6b4af1f3fd1");
        var newDefaultId = Guid.Parse("c7b298f2-9374-4ac1-bc52-25a3a58aab22");

        // Create the initial list of addresses using your factory method.
        var addresses = new List<Address>
    {
        Address.Create(
            id: initialDefaultId,
            customerId: customerId,
            addressLine1: "123 Maple Street",
            addressLine2: "Apt 4B",
            city: "New York",
            state: "NY",
            postalCode: "10001",
            country: "USA",
            isDefault: true
        ),
        Address.Create(
            id: newDefaultId,
            customerId: customerId,
            addressLine1: "44 Wall Street",
            addressLine2: "Suite 22",
            city: "New York",
            state: "NY",
            postalCode: "10005",
            country: "USA",
            isDefault: false
        )
    };

        var command = new SetDefaultAddressCommand(customerId, newDefaultId);

        // mocking the dependencies.
        var mockAddressRepo = new Mock<IAddressRepository>();
        var mockLogger = new Mock<ILogger<SetDefaultAddressCommandHandler>>();

        //  Set up the mock's behavior.
        mockAddressRepo.Setup(repo => repo.GetByCustomerAndAddressId(newDefaultId, customerId, It.IsAny<CancellationToken>(),false))
                       .ReturnsAsync(addresses.First(a => a.Id == newDefaultId));

        // When the update method is called, simulate the state change on our in-memory list.
        mockAddressRepo.Setup(repo => repo.SetDefaultAddressForCustomerAsync(customerId, newDefaultId, It.IsAny<CancellationToken>()))
                       .Callback(() =>
                       {
                           // Create a new list with updated instances (using 'with' expression for records)
                           var updatedList = addresses
                               .Select(a => a with { IsDefault = (a.Id == newDefaultId) })
                               .ToList();

                           // Replace the old list with the new one
                           addresses.Clear();
                           addresses.AddRange(updatedList);
                       });

        var handler = new SetDefaultAddressCommandHandler(_fixture.CustomerRepository, mockLogger.Object, mockAddressRepo.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        // Check the final state of our in-memory list.
        var oldDefaultAddress = addresses.First(a => a.Id == initialDefaultId);
        var newDefaultAddress = addresses.First(a => a.Id == newDefaultId);

        oldDefaultAddress.IsDefault.Should().BeFalse();
        newDefaultAddress.IsDefault.Should().BeTrue();

        mockAddressRepo.Verify(repo => repo.SetDefaultAddressForCustomerAsync(customerId, newDefaultId, It.IsAny<CancellationToken>()), Times.Once);

    }

    [Fact]
    public async Task Handle_CustomerDoesNotExist_ThrowsCustomerNotFoundException()
    {
        // Arrange
        var command = new SetDefaultAddressCommand(Guid.NewGuid(), Guid.NewGuid());

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<CustomerNotFoundException>();
    }

    [Fact]
    public async Task Handle_CustomerHasNoAddresses_ThrowsCustomerHasNoAddressesException()
    {
        // Arrange
        var customer = _fixture.Context.Customers.Include(c => c.Addresses).First();
        customer.Addresses.Clear();
        await _fixture.CustomerRepository.SaveChangesAsync(CancellationToken.None);

        var command = new SetDefaultAddressCommand(customer.Id, Guid.NewGuid());

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<CustomerHasNoAddressesException>();
    }

    [Fact]
    public async Task Handle_AddressDoesNotBelongToCustomer_ThrowsAddressDoesNotBelongToCustomerException()
    {
        // Arrange
        var customer = _fixture.Context.Customers.First();
        var address = _fixture.Context.Addresses.First(a => a.CustomerId != customer.Id);

        var command = new SetDefaultAddressCommand(customer.Id, address.Id);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AddressDoesNotBelongToCustomerException>();
    }
}
