using FluentAssertions;

namespace CustomerService.Tests.Application.Addresses.Commands;

public class EditAddressCommandHandlerTests : IClassFixture<TestFixtureBase<EditAddressCommandHandler>>
{
    private readonly TestFixtureBase<EditAddressCommandHandler> _fixture;
    private readonly EditAddressCommandHandler _handler;

    public EditAddressCommandHandlerTests(TestFixtureBase<EditAddressCommandHandler> fixture)
    {
        _fixture = fixture;
        _handler = new(
            _fixture.CustomerRepository,
            _fixture.AddressRepository,
            _fixture.Logger
        );
    }

    [Fact]
    public async Task Handle_ValidRequest_UpdatesAddressSuccessfully()
    {
        // Arrange
        var customer = _fixture.Context.Customers.AsNoTracking().First();
        var address = _fixture.Context.Addresses.AsNoTracking().First(a => a.CustomerId == customer.Id);

        var command = new EditAddressCommand(
            AddressId: address.Id,
            CustomerId: customer.Id,
            AddressLine1: "New Line 1",
            AddressLine2: "New Line 2",
            City: "Zarqa",
            State: "Zarqa",
            PostalCode: "22222",
            Country: "Jordan"
        );

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var updatedAddress = await _fixture.AddressRepository.GetByCustomerAndAddressId(address.Id, customer.Id, CancellationToken.None);
        updatedAddress.Should().NotBeNull();
        updatedAddress.AddressLine1.Should().Be(command.AddressLine1);
        updatedAddress.AddressLine2.Should().Be(command.AddressLine2);
        updatedAddress.City.Should().Be(command.City);
        updatedAddress.State.Should().Be(command.State);
        updatedAddress.PostalCode.Should().Be(command.PostalCode);
        updatedAddress.Country.Should().Be(command.Country);
    }

    [Fact]
    public async Task Handle_CustomerDoesNotExist_ThrowsCustomerNotFoundException()
    {
        // Arrange
        var command = new EditAddressCommand(
            AddressId: Guid.NewGuid(),
            CustomerId: Guid.NewGuid(),
            AddressLine1: "Line1",
            AddressLine2: "Line2",
            City: "City",
            State: "State",
            PostalCode: "12345",
            Country: "Country"
        );

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<CustomerNotFoundException>();
    }

    [Fact]
    public async Task Handle_AddressDoesNotBelongToCustomer_ThrowsAddressDoesNotBelongToCustomerException()
    {
        // Arrange
        var customer = _fixture.Context.Customers.First();
        var address = _fixture.Context.Addresses.First(a => a.CustomerId != customer.Id);

        var command = new EditAddressCommand(
            AddressId: address.Id,
            CustomerId: customer.Id,
            AddressLine1: "New Line",
            AddressLine2: "New Line 2",
            City: "New City",
            State: "New State",
            PostalCode: "99999",
            Country: "Jordan"
        );

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AddressDoesNotBelongToCustomerException>();
    }
}
