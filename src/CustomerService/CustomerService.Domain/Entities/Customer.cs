using CustomerService.Domain.Errors;
using CustomerService.Domain.ValueObjects;
using SharedKernel.Primitives.Result;
using SharedKernel.Shared;

namespace CustomerService.Domain.Entities;

public sealed record Customer : AggregateRoot
{
    public IReadOnlyCollection<Address> Addresses { get => _addresses.AsReadOnly(); }

    private Customer(
            Guid id,
            Guid userId,
            FullName fullName,
            PhoneNumber phoneNumber,
            List<Address> addresses) : base(id)
    {
        UserId = userId;
        FullName = fullName;
        PhoneNumber = phoneNumber;
        _addresses = addresses;
    }

    private Customer() {}
    

    public Guid UserId { get; init; }
    public FullName FullName { get; init; }
    public PhoneNumber PhoneNumber { get; init; }
    private readonly List<Address> _addresses = new();


    /// <summary>
    /// Factory method to create a new Customer instance with validation.
    /// </summary>
    public static Result<Customer> Create(
        Guid userId,
        string firstName,
        string lastName,
        string phoneNumber,
        List<Address>? addresses = null)
    {
        
        if (userId == Guid.Empty) return DomainErrors.Customer.EmptyUserId;

        return new Customer(
            Guid.NewGuid(),
            userId,
            FullName.Create(firstName, lastName).Value,
            PhoneNumber.Create(phoneNumber).Value,
            addresses ?? new());
    }

    
    public Result<Customer> Edit(string firstName, string lastNamE, string phoneNumber)
    {
        return this with
        {
            FullName = FullName.Create(firstName, lastNamE).Value,
            PhoneNumber = PhoneNumber.Create(phoneNumber).Value
        };
    }

public Result SetDefaultAddress(Guid addressId)
    {
        var targetAddress = _addresses.FirstOrDefault(a => a.Id == addressId);

        if (targetAddress is null)
        {
            return DomainErrors.Address.NotFound(addressId);
        }

        foreach (var address in _addresses)
        {
            if (address.Id == addressId)
            {
                address.SetAsDefault();
            }
            else
            {
                address.UnsetDefault();
            }
        }

        return Result.Success();
    }
    public Result AddAddress(Address address)
    {
        if (_addresses.Any(a => a.Id == address.Id))
            return DomainErrors.Customer.AddressAlreadyExists(address.Id);

        _addresses.Add(address);
        return Result.Success();
    }

    public Result DeleteAddress(Guid addressId)
    {
        if (_addresses.Any(a => a.Id == addressId))
        {
            var addressToRemove = _addresses.First(a => a.Id == addressId);
            _addresses.Remove(addressToRemove);
            return Result.Success();
        }

        return DomainErrors.Address.NotFound(addressId);
    }

    public void ClearAddresses()
    {
        _addresses.Clear();
    }

    public Result UpdateAddress(Guid addressId, Address address)
    {
        var existingAddress = _addresses.FirstOrDefault(a => a.Id == addressId);
        if (existingAddress is null)
        {
            return DomainErrors.Address.NotFound(addressId);
        }

        _addresses.Remove(existingAddress);
        _addresses.Add(address);
        return Result.Success();
    }

 
}
