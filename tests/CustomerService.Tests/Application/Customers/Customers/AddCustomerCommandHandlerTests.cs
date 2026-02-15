using FluentAssertions;

namespace CustomerService.Tests.Application.Customers.Commands;

public class AddCustomerCommandHandlerTests : IClassFixture<TestFixtureBase<AddCustomerCommandHandler>>
{
    private readonly TestFixtureBase<AddCustomerCommandHandler> _fixture;
    private readonly AddCustomerCommandHandler _handler;

    public AddCustomerCommandHandlerTests(TestFixtureBase<AddCustomerCommandHandler> fixture)
    {
        _fixture = fixture;
        _handler = new(
            _fixture.CustomerRepository,
            fixture.Logger,
            fixture.Mapper
        );
    }

    [Fact]
    public async Task Handle_ValidCommand_AddsCustomerSuccessfully()
    {
        var userId = Guid.NewGuid();
        // Arrange
        var command = new AddCustomerCommand(
            UserId: userId,
            FirstName: "Omar",
            LastName: "Abumuhfouz",
            PhoneNumber: "+962-790-123456"
        );

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var customer = await _fixture.CustomerRepository.GetByUserIdAsync(userId, CancellationToken.None);
        customer.Should().NotBeNull();

        customer.FullName.FirstName.Should().Be(command.FirstName);
        customer.FullName.LastName.Should().Be(command.LastName);
        customer.PhoneNumber.Value.Should().Be(command.PhoneNumber);
    }

    [Fact]
    public async Task Handle_InvalidUserId_ThrowsArgumentException()
    {
        // Arrange
        var command = new AddCustomerCommand(
            UserId: Guid.Empty,
            FirstName: "Test",
            LastName: "User",
            PhoneNumber: "+962-790-111111"
        );

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task Handle_DuplicateCustomer_ThrowsDuplicateCustomerException()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var firstCommand = new AddCustomerCommand(userId, "Omar", "Abumuhfouz", "+962-790-123456");

        var firstcustomer= await _handler.Handle(firstCommand, CancellationToken.None);
        firstcustomer.Should().NotBeEmpty();

        var secondCommand = new AddCustomerCommand(userId, "Omar", "Abumuhfouz", "+962-790-123456");

        var act = async () => await _handler.Handle(secondCommand, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<DuplicateCustomerException>();
    }
}