using CustomerService.Domain.Constants;
using CustomerService.Domain.Errors;
using SharedKernel.Shared;

namespace CustomerService.Domain.ValueObjects;

public sealed record FullName 
{
    public string FirstName { get; }
    public string LastName { get; }

    private FullName() { } // For EF Core

    private  FullName(string firstName, string lastName)
    {
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
    }

    public static Result<FullName> Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName)) return DomainErrors.FullName.FirstNameEmpty;

        if(firstName.Length > CustomerConstants.MAX_FIRST_NAME_LENGTH) return DomainErrors.FullName.FirstNameTooLong;

        if (string.IsNullOrWhiteSpace(lastName)) return DomainErrors.FullName.LastNameEmpty;

        if(lastName.Length > CustomerConstants.MAX_LAST_NAME_LENGTH) return DomainErrors.FullName.LastNameTooLong;

        return new FullName(firstName, lastName);
    }

    public override string ToString() => $"{FirstName} {LastName}";

    
}
