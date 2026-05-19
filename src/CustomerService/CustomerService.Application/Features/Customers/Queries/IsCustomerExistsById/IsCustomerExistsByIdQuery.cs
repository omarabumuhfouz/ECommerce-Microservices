namespace CustomerService.Application.Features.Customers.Queries.IsCustomerExistsById;

public record IsCustomerExistsByIdQuery(Guid CustomerId) :
    ICachedQuery<Unit>
{
    public string CacheKey => CacheKeys.KeyExists(CustomerId);

    public string[] Tags => CacheKeys.TagsById(CustomerId);

    public TimeSpan Expiration => CacheKeys.Expiration;
}