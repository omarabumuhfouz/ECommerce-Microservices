using CustomerService.Application.Addresses.DTOs;

namespace CustomerService.Application.Features.Addresses.Queries.GetAddressesByCustomer;

public record GetAddressesByCustomerQuery(Guid CustomerId)
        : ICachedQuery<List<AddressDto>>
{
    public string CacheKey => CacheKeys.KeyByIdAndAddresses(CustomerId);

    public string[] Tags => CacheKeys.TagsById(CustomerId);

    public TimeSpan Expiration => CacheKeys.Expiration;
}