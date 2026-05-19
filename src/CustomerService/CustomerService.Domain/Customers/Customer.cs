using CustomerService.Domain.Errors;
using SharedKernel.Primitives;
using SharedKernel.Primitives.Results;

namespace CustomerService.Domain.Customers;

public sealed class Customer : AggregateRoot, IAuditableEntity
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

    #pragma warning disable CS8618 // Optional: if you want to be extra explicit
    private Customer() { }
    #pragma warning restore CS8618
    

    public Guid UserId { get;  set; }
    public FullName FullName { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public DateTime CreatedOnUtc { get;  set ; }
    public DateTime? ModifiedOnUtc { get ; set ; }

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



    public Result Update(string firstName, string lastName, string phoneNumber)
    {
        return Result.Success()
            .Bind(_ => UpdatePhoneNumber(phoneNumber))
            .Bind(_ => UpdateFullName(firstName, lastName));
    }

    private Result UpdatePhoneNumber(string phoneNumber)
    {
        if (PhoneNumber.Value == phoneNumber) return Result.Success();

        return PhoneNumber.Create(phoneNumber)
            .Tap(newPhoneNumber => PhoneNumber = newPhoneNumber);
    }

    private Result UpdateFullName(string firstName, string lastName)
    {
        // Check if either part of the name changed
        if (FullName.FirstName == firstName && FullName.LastName == lastName)
            return Result.Success();

        return FullName.Create(firstName, lastName)
            .Tap(newName =>
            {
                FullName = newName;
                RaiseDomainEvent(new ChangeCustomerNameDomainEvent(this.Id, FullName.ToString()));
            });
    }

    public Result SetDefaultAddress(Guid addressId)
    {
        var targetAddress = _addresses.FirstOrDefault(a => a.Id == addressId);

        if (targetAddress is null) return DomainErrors.Address.NotFound(addressId);

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
        if (_addresses.Any(a => a.Id == address.Id)) return DomainErrors.Customer.AddressAlreadyExists(address.Id);

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

    public void ClearAddresses() => _addresses.Clear();

    public Result UpdateAddress(Guid addressId, Address address)
    {
        var existingAddress = _addresses.FirstOrDefault(a => a.Id == addressId);
        if (existingAddress is null) return DomainErrors.Address.NotFound(addressId);

        _addresses.Remove(existingAddress);
        _addresses.Add(address);
        return Result.Success();
    }

 
}
