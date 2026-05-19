namespace CustomerService.Application.Features.Addresses.Commands.DeleteAddress;

public class DeleteAddressCommandValidator : AbstractValidator<DeleteAddressCommand>
{
    public DeleteAddressCommandValidator()
    {
        RuleFor(d => d.CustomerId).ValidateCustomerId();
        RuleFor(d => d.AddressId).ValidateAddressId();

        
    }
}