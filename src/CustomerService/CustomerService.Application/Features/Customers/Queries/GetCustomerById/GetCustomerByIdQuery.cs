namespace CustomerService.Application.Features.Customers.Queries.GetCustomerById;

public record GetCustomerByIdQuery(Guid CustomerId)
        : ICachedQuery<CustomerDetailsDto>
{
    public string CacheKey => CacheKeys.KeyById(CustomerId);

    public string[] Tags => CacheKeys.TagsById(CustomerId);

    public TimeSpan Expiration => CacheKeys.Expiration;
}