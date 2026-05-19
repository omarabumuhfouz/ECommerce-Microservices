namespace CustomerService.Application.DTOs;

public class CustomerSeedDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }

    public List<AddressSeedDto> Addresses { get; set; } = new();
}

public class AddressSeedDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string AddressLine1 { get; set; } = default!;
    public string AddressLine2 { get; set; } = default!;
    public string City { get; set; } = default!;
    public string State { get; set; } = default!;
    public string PostalCode { get; set; } = default!;
    public string Country { get; set; } = default!;
    public bool IsDefault { get; set; }
}
