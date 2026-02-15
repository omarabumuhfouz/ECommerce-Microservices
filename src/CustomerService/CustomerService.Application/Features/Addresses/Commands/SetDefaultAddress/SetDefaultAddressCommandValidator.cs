namespace CustomerService.Application.Addresses.Commands.SetDefaultAddress;

public class SetDefaultAddressCommandValidator : AbstractValidator<SetDefaultAddressCommand>
{
    public SetDefaultAddressCommandValidator()
    {
        RuleFor(d => d.CustomerId).ValidateCustomerId();
        RuleFor(d => d.AddressId).ValidateAddressId();

        
    }
}