namespace CustomerService.Application.Features.Addresses.Commands.DeleteAddress;

public sealed record DeleteAddressCommand(Guid AddressId, Guid CustomerId)
        : ICommand<Unit>, IInvalidateCache
{
    public string[] Tags => CacheKeys.TagsById(CustomerId);
}