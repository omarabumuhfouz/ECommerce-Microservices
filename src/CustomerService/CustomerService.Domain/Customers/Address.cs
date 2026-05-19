using CustomerService.Domain.Errors;
using SharedKernel.Primitives;
using SharedKernel.Primitives.Results;

namespace CustomerService.Domain.Customers;

public sealed class Address : Entity
{
    private Address(
           Guid id,
           Guid customerId,
           string addressLine1,
           string addressLine2,
           string city,
           string state,
           string postalCode,
           string country,
           bool isDefault) : base(id)
    {
        CustomerId = customerId;
        AddressLine1 = addressLine1;
        AddressLine2 = addressLine2;
        City = city;
        State = state;
        PostalCode = postalCode;
        Country = country;
        IsDefault = isDefault;
    }

    #pragma warning disable CS8618 // Optional: if you want to be extra explicit
    private Address() { }
    #pragma warning restore CS8618

    public string AddressLine1 { get; private set; } = string.Empty;
    public string AddressLine2 { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public string PostalCode { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;
    public bool IsDefault { get; private set; } = false;

    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; }

    /// <summary>
    /// Factory method to create a new Address with validation.
    /// </summary>
    public static Result<Address> Create(
        Guid id,
        Guid customerId,
        string addressLine1,
        string addressLine2,
        string city,
        string state,
        string postalCode,
        string country,
        bool isDefault = false)
    {
        if (id == Guid.Empty) return DomainErrors.Address.EmptyId;

        if (customerId == Guid.Empty) return DomainErrors.Customer.EmptyId;

        if (string.IsNullOrWhiteSpace(addressLine1)) return DomainErrors.Address.EmptyAddressLine1;

        if (string.IsNullOrWhiteSpace(addressLine2)) return DomainErrors.Address.EmptyAddressLine2;

        if (string.IsNullOrWhiteSpace(city)) return DomainErrors.Address.EmptyCity;

        if (string.IsNullOrWhiteSpace(state)) return DomainErrors.Address.EmptyState;

        if (string.IsNullOrWhiteSpace(postalCode)) return DomainErrors.Address.EmptyPostalCode;

        if (string.IsNullOrWhiteSpace(country)) return DomainErrors.Address.EmptyCountry;


        return new Address(
            id,
            customerId,
            addressLine1.Trim(),
            addressLine2.Trim(),
            city.Trim(),
            state.Trim(),
            postalCode.Trim(),
            country.Trim(),
            isDefault
        );
    }

    public void SetAsDefault() => IsDefault = true;

    public void UnsetDefault() => IsDefault = false;

    // internal Address UpdateAddress(string addressLine1, string addressLine2, string city, string state, string country, string postalCode, bool isDefault)
    // {
    //     return this with
    //     {
    //         AddressLine1 = addressLine1,
    //         AddressLine2 = addressLine2,
    //         City = city,
    //         State = state,
    //         PostalCode = postalCode,
    //         Country = country,
    //         IsDefault = isDefault
    //     };
    // }
}
