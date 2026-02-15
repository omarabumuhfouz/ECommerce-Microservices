using Contracts.Customer;
using OrderService.Domain.Errors;

namespace OrderService.Domain.DTOs;

public record CustomerDto(
    Guid Id,
    Guid UserId,
    string FirstName,
    string LastName,
    string PhoneNumber,
    List<AddressDto> Addresses
)
{
    public Result<AddressDto> GetAddressById(Guid addressId)
    {
        var address = Addresses.FirstOrDefault(a => a.Id == addressId);
        if (address is null) return DomainErrors.Customer.AddressNotFound(addressId);
        return address;
    }

    public static CustomerDto FromGrpcModel(CustomerModel model)
    {
        return new CustomerDto(
            Guid.Parse(model.Id),
            Guid.Parse(model.UserId),
            model.FirstName,
            model.LastName,
            model.PhoneNumber,
            model.Addresses.Select(a => AddressDto.FromGrpcModel(a)).ToList()
        );

    }
}