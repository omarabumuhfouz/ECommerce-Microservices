using FluentAssertions;

namespace CustomerService.Tests.Application.Customers.Commands;

public class EditCustomerCommandHandlerTests : IClassFixture<TestFixtureBase<EditCustomerCommandHandler>>
{
    private readonly TestFixtureBase<EditCustomerCommandHandler> _fixture;
    private readonly EditCustomerCommandHandler _handler;

    public EditCustomerCommandHandlerTests(TestFixtureBase<EditCustomerCommandHandler> fixture)
    {
        _fixture = fixture;
        _handler = new(
            _fixture.CustomerRepository,
            fixture.Logger
        );
    }

    [Fact]
    public async Task Handle_ValidCustomer_UpdatesCustomerSuccessfully()
    {
        // Arrange
        var existingCustomer = _fixture.Context.Customers.AsNoTracking().First();
        var command = new EditCustomerCommand(
            existingCustomer.UserId,
            "UpdatedFirst",
            "UpdatedLast",
            "+962-790-987654"
        );

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var updatedCustomer = await _fixture.CustomerRepository.GetByUserIdAsync(existingCustomer.UserId, CancellationToken.None);
        updatedCustomer.Should().NotBeNull();
        updatedCustomer.FullName.FirstName.Should().Be(command.FirstName);
        updatedCustomer.FullName.LastName.Should().Be(command.LastName);
        updatedCustomer.PhoneNumber.Value.Should().Be(command.PhoneNumber);
    }

    [Fact]
    public async Task Handle_CustomerNotFound_ThrowsCustomerNotFoundByUserException()
    {
        // Arrange
        var command = new EditCustomerCommand(
            Guid.NewGuid(),
            "Ghost",
            "Customer",
            "+962-790-000000"
        );

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<CustomerNotFoundByUserException>();
    }
}
