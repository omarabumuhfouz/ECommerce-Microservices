using CustomerService.Domain.Errors;
using SharedKernel.Primitives.Results;

namespace CustomerService.Domain.Customers;

public sealed class PhoneNumber
{
    public string Value { get; }

    private PhoneNumber() { } // For EF Core

    private PhoneNumber(string value) => Value = value.Trim();
    

    public static Result<PhoneNumber> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return DomainErrors.PhoneNumber.Empty;

        // if (Regex.IsMatch(value, @"^\+?[1-9]\d{7,14}$") == false)
        //     return DomainErrors.PhoneNumber.InvalidFormat;

        return new PhoneNumber(value);
    }

    public override string ToString() => Value;
}
