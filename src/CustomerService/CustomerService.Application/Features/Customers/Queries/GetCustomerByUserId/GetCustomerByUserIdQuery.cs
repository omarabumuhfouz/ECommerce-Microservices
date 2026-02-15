using SharedKernel.Abstractions.Messaging;

namespace CustomerService.Application.Customers.Queries;

public record GetCustomerByUserIdQuery(Guid UserId) : ICachedQuery<CustomerDetailsDto>
{
    public string CacheKey => CacheKeys.KeyByUserId(UserId);

    public string[] Tags => CacheKeys.TagsByUserId(UserId);

    public TimeSpan Expiration => CacheKeys.Expiration;
}