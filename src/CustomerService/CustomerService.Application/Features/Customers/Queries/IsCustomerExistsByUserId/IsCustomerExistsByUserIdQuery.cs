namespace CustomerService.Application.Features.Customers.Queries.IsCustomerExistsByUserId;

public record IsCustomerExistsByUserIdQuery(Guid UserId) : ICachedQuery<Unit>
{
    public string CacheKey => CacheKeys.KeyExists(UserId);

    public string[] Tags => CacheKeys.TagsByUserId(UserId);

    public TimeSpan Expiration => CacheKeys.Expiration;
}