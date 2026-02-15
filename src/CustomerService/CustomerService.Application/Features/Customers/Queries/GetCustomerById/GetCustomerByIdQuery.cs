using SharedKernel.Abstractions.Messaging;

namespace CustomerService.Application.Customers.Queries;

public record GetCustomerByIdQuery(Guid CustomerId)
        : ICachedQuery<CustomerDetailsDto>
{
    public string CacheKey => CacheKeys.KeyById(CustomerId);

    public string[] Tags => CacheKeys.TagsById(CustomerId);

    public TimeSpan Expiration => CacheKeys.Expiration;
}