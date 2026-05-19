namespace CustomerService.Application.Features.Addresses.Commands.CreateAddress;

public class CreateAddressCommandValidator : AbstractValidator<CreateAddressCommand>
{
    public CreateAddressCommandValidator()
    {
        RuleFor(x => x.CustomerId).ValidateCustomerId();
        RuleFor(x => x.AddressLine1).ValidateAddressLine1();
        RuleFor(x => x.AddressLine2).ValidateAddressLine2();
        RuleFor(x => x.City).ValidateCity();
        RuleFor(x => x.State).ValidateState();
        RuleFor(x => x.PostalCode).ValidatePostalCode();
        RuleFor(x => x.Country).ValidateCountry();
    }
}
