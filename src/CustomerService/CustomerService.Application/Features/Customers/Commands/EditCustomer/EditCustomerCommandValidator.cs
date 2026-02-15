namespace CustomerService.Application.Customers.Commands.EditCustomer;

public class EditCustomerCommandValidator : AbstractValidator<EditCustomerCommand>
{
    public EditCustomerCommandValidator()
    {
        RuleFor(x => x.CustomerId).ValidateCustomerId();
        RuleFor(x => x.FirstName).ValidateFirstName();
        RuleFor(x => x.LastName).ValidateLastName();
        RuleFor(x => x.PhoneNumber).ValidatePhoneNumber();
    }
}