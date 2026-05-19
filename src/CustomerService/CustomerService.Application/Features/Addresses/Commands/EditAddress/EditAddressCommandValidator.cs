namespace CustomerService.Application.Features.Addresses.Commands.EditAddress;

public class EditAddressCommandValidator : AbstractValidator<EditAddressCommand>
{
    public EditAddressCommandValidator()
    {
        RuleFor(d => d.AddressId).ValidateAddressId();
        RuleFor(x => x.CustomerId).ValidateCustomerId();
        RuleFor(x => x.AddressLine1).ValidateAddressLine1();
        RuleFor(x => x.AddressLine2).ValidateAddressLine2();
        RuleFor(x => x.City).ValidateCity();
        RuleFor(x => x.State).ValidateState();
        RuleFor(x => x.PostalCode).ValidatePostalCode();
        RuleFor(x => x.Country).ValidateCountry();

        
    }
}