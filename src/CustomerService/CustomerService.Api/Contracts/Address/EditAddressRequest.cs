using CustomerService.Application.Features.Addresses.Commands.EditAddress;

namespace CustomerService.Api.Contracts.Address;

public record EditAddressRequest(
    string AddressLine1,
    string AddressLine2,
    string City,
    string State,
    string PostalCode,
    string Country,
    bool IsDefault

)
{
    public EditAddressCommand ToCommand(Guid customerId, Guid addressId)
    {
        return new EditAddressCommand(
            customerId,
            addressId,
            AddressLine1,
            AddressLine2,
            City,
            State,
            PostalCode,
            Country,
            IsDefault
        );
    }
}