namespace CustomerService.Application.Addresses.DTOs;

public record AddressDto
(
    Guid CustomerId,
    Guid AddressId,
    string AddressLine1,
    string AddressLine2,
    string City,
    string State,
    string PostalCode,
    string Country,
    bool IsDefault
);
