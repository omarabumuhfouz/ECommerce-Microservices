namespace CustomerService.Application.Customers.Queries;

public record IsCustomerExistsByIdQuery(Guid CustomerId) :
    ICachedQuery<Unit>
{
    public string CacheKey => CacheKeys.KeyExists(CustomerId);

    public string[] Tags => CacheKeys.TagsById(CustomerId);

    public TimeSpan Expiration => CacheKeys.Expiration;
}