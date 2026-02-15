namespace SharedKernel.Constants;

public static class CacheKeyProvider
{
    public static string GetCustomerKey(Guid customerId) => "customer-{customerId}";
    public static string CustomerByUserIdKey(Guid userId) => "customer-by-userId-{userId}";
}
