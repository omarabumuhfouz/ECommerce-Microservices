namespace CustomerService.Application.Features.Customers.Queries.GetCustomerByUserId;

public record GetCustomerByUserIdQuery(Guid UserId) : ICachedQuery<CustomerDetailsDto>
{
    public string CacheKey => CacheKeys.KeyByUserId(UserId);

    public string[] Tags => CacheKeys.TagsByUserId(UserId);

    public TimeSpan Expiration => CacheKeys.Expiration;
}