namespace PaymentService.Domain.Constants;

public static class CacheKeys
{
    public const string GROUP_TAG = "customer";
    public static readonly TimeSpan Expiration = TimeSpan.FromMinutes(30);

    public static readonly Func<Guid, string> KeyById =
        id => $"customer-{id}";

    public static readonly Func<Guid, string> KeyByUserId =
        id => $"customer-user-{id}";

    public static readonly Func<Guid, string> KeyExists =
        id => $"customer-exists-{id}";

    public static readonly Func<Guid, string[]> TagsById =
        id => [$"customer-{id}"];

    public static readonly Func<Guid, string[]> TagsByUserId =
        id => [$"customer-user-{id}"];

    public static readonly string[] GroupTags = new[] { GROUP_TAG };
}