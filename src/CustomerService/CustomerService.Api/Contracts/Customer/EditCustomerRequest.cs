using CustomerService.Application.Features.Customers.Commands.EditCustomer;

namespace CustomerService.Api.Contracts.Customer;

public record EditCustomerRequest(string FirstName, string LastName, string PhoneNumber)
{
    public EditCustomerCommand ToCommand(Guid userId)
     => new EditCustomerCommand(userId, FirstName, LastName, PhoneNumber);
}
