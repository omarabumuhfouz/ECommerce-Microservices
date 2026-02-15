using FluentAssertions;

namespace CustomerService.Tests.Application.Addresses.Commands;

public class DeleteAddressCommandHandlerTests : IClassFixture<TestFixtureBase<DeleteAddressCommandHandler>>
{
    private readonly TestFixtureBase<DeleteAddressCommandHandler> _fixture;
    private readonly DeleteAddressCommandHandler _handler;

    public DeleteAddressCommandHandlerTests(TestFixtureBase<DeleteAddressCommandHandler> fixture)
    {
        _fixture = fixture;
        _handler = new(
            _fixture.CustomerRepository,
            _fixture.AddressRepository,
            _fixture.Mapper,
            _fixture.Logger
        );
    }

    [Fact]
    public async Task Handle_ValidRequest_DeletesAddressSuccessfully()
    {
        // Arrange
        var customer = _fixture.Context.Customers.AsNoTracking().First();
        var address = _fixture.Context.Addresses.AsNoTracking().First(a => a.CustomerId == customer.Id);

        var command = new DeleteAddressCommand(address.Id, customer.Id);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var deletedAddress = await _fixture.AddressRepository.GetByCustomerAndAddressId(address.Id, customer.Id, CancellationToken.None);
        deletedAddress.Should().BeNull();
    }

    [Fact]
    public async Task Handle_CustomerDoesNotExist_ThrowsCustomerNotFoundException()
    {
        // Arrange
        var command = new DeleteAddressCommand(Guid.NewGuid(), Guid.NewGuid());

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

        var command = new DeleteAddressCommand(address.Id, customer.Id);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AddressDoesNotBelongToCustomerException>();
    }
}
