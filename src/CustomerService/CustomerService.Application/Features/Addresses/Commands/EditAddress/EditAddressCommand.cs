namespace CustomerService.Application.Features.Addresses.Commands.EditAddress;

public sealed record EditAddressCommand(
    Guid CustomerId,
    Guid AddressId,
    string AddressLine1,
    string AddressLine2,
    string City,
    string State,
    string PostalCode,
    string Country,
    bool IsDefault

) : ICommand<Unit>, IInvalidateCache
{
    public string[] Tags => CacheKeys.TagsById(CustomerId);
}