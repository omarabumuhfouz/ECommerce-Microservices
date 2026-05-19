using CustomerService.Application.Features.Addresses.Commands.CreateAddress;

namespace CustomerService.Api.Contracts.Address;

public record CreateAddressRequest(
    string AddressLine1,
    string AddressLine2,
    string City,
    string State,
    string PostalCode,
    string Country,
    bool IsDefault

)
{
    public CreateAddressCommand ToCommand(Guid customerId)
    {
        return new CreateAddressCommand(
            customerId,
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
