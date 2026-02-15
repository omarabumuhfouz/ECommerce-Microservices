namespace CustomerService.Application.Customers.Commands.EditCustomer;

public record EditCustomerCommand(Guid CustomerId, string FirstName, string LastName, string PhoneNumber)
 : ICommand<Unit>, IInvalidateCache
{
    public string[] Tags => CacheKeys.TagsById(CustomerId);
}