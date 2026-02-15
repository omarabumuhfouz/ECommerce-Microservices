using FluentAssertions;

namespace CustomerService.Tests.Application.Addresses.Commands;

public class CreateAddressCommandHandlerTests : IClassFixture<TestFixtureBase<CreateAddressCommandHandler>>
{
    private readonly TestFixtureBase<CreateAddressCommandHandler> _fixture;
    private readonly CreateAddressCommandHandler _handler;

    public CreateAddressCommandHandlerTests(TestFixtureBase<CreateAddressCommandHandler> fixture)
    {
        _fixture = fixture;
        _handler = new(
            _fixture.CustomerRepository,
            _fixture.AddressRepository,
            _fixture.Mapper,
            fixture.Logger
        );
    }

    [Fact]
    public async Task Handle_ValidRequest_AddsAddressSuccessfully()
    {
        // Arrange
        var customer = _fixture.Context.Customers.First();

        var command = new CreateAddressCommand(
            CustomerId: customer.Id,
            AddressLine1: "123 Main St",
            AddressLine2: "Apartment 4B",
            City: "Amman",
            State: "Amman",
            PostalCode: "11111",
            Country: "Jordan",
            IsDefault: true
        );

        // Act
        var addressId = await _handler.Handle(command, CancellationToken.None);

        // Assert
        addressId.Should().NotBeEmpty();
        var address = _fixture.Context.Addresses.FirstOrDefault(a => a.Id == addressId);
        address.Should().NotBeNull();
        address.AddressLine1.Should().Be(command.AddressLine1);
        address.AddressLine2.Should().Be(command.AddressLine2);
        address.City.Should().Be(command.City);
        address.IsDefault.Should().Be(command.IsDefault);
    }

    [Fact]
    public async Task Handle_CustomerDoesNotExist_ThrowsCustomerNotFoundException()
    {
        // Arrange
        var command = new CreateAddressCommand(
            CustomerId: Guid.NewGuid(),
            AddressLine1: "123 Main St",
            AddressLine2: "Apartment 4B",
            City: "Amman",
            State: "Amman",
            PostalCode: "11111",
            Country: "Jordan",
            IsDefault: true
        );

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<CustomerNotFoundException>();
    }
}
