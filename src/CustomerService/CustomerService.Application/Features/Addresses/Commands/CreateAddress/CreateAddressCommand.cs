namespace CustomerService.Application.Features.Addresses.Commands.CreateAddress;

public sealed record CreateAddressCommand(
    Guid CustomerId,
    string AddressLine1,
    string AddressLine2,
    string City,
    string State,
    string PostalCode,
    string Country,
    bool IsDefault

)
 : ICommand<Guid>, IInvalidateCache
{
    public string[] Tags => CacheKeys.TagsById(CustomerId);
}