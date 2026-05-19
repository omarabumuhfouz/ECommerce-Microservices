namespace CustomerService.Application.Features.Customers.Commands.AddCustomer;

public class AddCustomerCommandValidator : AbstractValidator<AddCustomerCommand>
{
    public AddCustomerCommandValidator()
    {
        RuleFor(x => x.UserId).ValidateUserId();
        RuleFor(x => x.FirstName).ValidateFirstName();
        RuleFor(x => x.LastName).ValidateLastName();
        RuleFor(x => x.PhoneNumber).ValidatePhoneNumber();
    }
}
