namespace CustomerService.Application.Addresses.Commands.SetDefaultAddress;

public sealed record SetDefaultAddressCommand(Guid CustomerId, Guid AddressId)
    : ICommand<Unit>, IInvalidateCache
{
    public string[] Tags => CacheKeys.TagsById(CustomerId);
}