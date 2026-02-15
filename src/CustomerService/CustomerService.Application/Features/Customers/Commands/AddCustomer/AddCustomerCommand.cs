using SharedKernel.Abstractions.Messaging;

namespace CustomerService.Application.Customers.Commands.AddCustomer;

public record AddCustomerCommand(Guid UserId, string FirstName, string LastName, string PhoneNumber)
 : ICommand<Guid>;