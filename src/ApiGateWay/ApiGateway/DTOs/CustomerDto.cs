using Contracts.Customer;

namespace ApiGateway.DTOs;

public record CustomerDto(
    Guid Id,
    Guid UserId,
    string FirstName,
    string LastName,
    string PhoneNumber,
    List<AddressDto> Addresses
    )
{
    public static CustomerDto FromCustomerModel(CustomerModel model)
    {
        return null;
    }
    
}