using CustomerService.Application.Addresses.DTOs;

namespace CustomerService.Application.Customers.DTOs;

public record CustomerDetailsDto(Guid Id, Guid UserId, string FirstName, string LastName, string PhoneNumber, List<AddressDto> Addresses);
