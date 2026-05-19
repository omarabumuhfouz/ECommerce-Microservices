
using CustomerService.Domain.Constants;
using SharedKernel.Primitives.Results;

namespace CustomerService.Domain.Errors;

public static class DomainErrors
{
    public static class Customer
    {
        public static readonly Error EmptyUserId = new(
            ErrorCodes.Customer.USERID_REQUIRED,
            "User Id must be provided.",
            ErrorType.Validation);
        
        public static readonly Func<Guid, Error> UserIdAlreadyExists = userId => new Error(
            ErrorCodes.Customer.USERID_ALREADY_EXISTS,
            $"A customer with the UserId '{userId}' already exists.",
            ErrorType.Conflict);

        public static readonly Error EmptyId = new(
            ErrorCodes.Customer.ID_REQUIRED,
            "Id must be provided.",
            ErrorType.Validation);

        public static readonly Func<Guid, Error> NotFound = id => new Error(
            ErrorCodes.Customer.NOTFOUND,
            $"The member with the identifier {id} was not found.",
            ErrorType.NotFound);

        public static readonly Func<Guid, Error> NotFoundByUser = id => new Error(
            ErrorCodes.Customer.NOTFOUND_BY_USER,
            $"The customer with the UserId {id} was not found.",
            ErrorType.NotFound);

        public static readonly Func<Guid, Error> AddressAlreadyExists = id => new Error(
                ErrorCodes.Customer.ADDRESS_ALREADY_EXISTS,
                $"The address with id: {id}, already exists for this customer.",
                ErrorType.Conflict);
    }

    public static class Address
    {
        public static readonly Func<Guid, Error> NotFound = id => new Error(
           ErrorCodes.Address.NOTFOUND,
            $"The address with id: {id}, was not found for this customer.",
            ErrorType.NotFound
        );

        public static readonly Error EmptyId = new(
            ErrorCodes.Address.ID_REQUIRED,
            "Address Id must be provided.",
            ErrorType.Validation);

        public static readonly Error EmptyAddressLine1 = new(
                    ErrorCodes.Address.LINE1_REQUIRED,
                    "AddressLine1 cannot be null or empty.",
                    ErrorType.Validation);

        public static readonly Error EmptyAddressLine2 = new(
            ErrorCodes.Address.LINE2_REQUIRED,
            "AddressLine2 cannot be null or empty.",
            ErrorType.Validation);

        public static readonly Error EmptyCity = new(
            ErrorCodes.Address.CITY_REQUIRED,
            "City cannot be null or empty.",
            ErrorType.Validation);

        public static readonly Error EmptyState = new(
            ErrorCodes.Address.STATE_REQUIRED,
            "State cannot be null or empty.",
            ErrorType.Validation);

        public static readonly Error EmptyPostalCode = new(
            ErrorCodes.Address.POSTALCODE_REQUIRED,
            "PostalCode cannot be null or empty.",
            ErrorType.Validation);

        public static readonly Error EmptyCountry = new(
            ErrorCodes.Address.COUNTRY_REQUIRED,
            "Country cannot be null or empty.",
            ErrorType.Validation);
    }

    public static class PhoneNumber
    {
        public static readonly Error Empty = new(
            ErrorCodes.PhoneNumber.IS_REQUIRED,
            "PhoneNumber is empty",
            ErrorType.Validation);

        public static readonly Error InvalidFormat = new(
            ErrorCodes.PhoneNumber.INVALID_FORMAT,
            "PhoneNumber format is invalid",
            ErrorType.Validation);
    }

    public static class FullName
    {
        public static readonly Error FirstNameEmpty = new(
            ErrorCodes.FirstName.IS_REQUIRED,
            "First name is empty",
            ErrorType.Validation);

        public static readonly Error FirstNameTooLong = new(
            ErrorCodes.FirstName.MAX_LENGTH,
            "FirstName name is too long",
            ErrorType.Validation);

        public static readonly Error LastNameEmpty = new(
            ErrorCodes.LastName.IS_REQUIRED,
            "Last name is empty",
            ErrorType.Validation);

        public static readonly Error LastNameTooLong = new(
            ErrorCodes.LastName.MAX_LENGTH,
            "Last name is too long",
            ErrorType.Validation);

    }
}
