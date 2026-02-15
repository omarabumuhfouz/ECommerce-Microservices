using Microsoft.AspNetCore.StaticAssets;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CustomerService.Domain.Constants;

public static class ErrorCodes
{
    public static class Customer
    {
        public static readonly string ID_REQUIRED = "Customer.IdRequired";
        public static readonly string USERID_REQUIRED = "Customer.UserIdRequired";
        public static readonly string FIRST_NAME_REQUIRED = "Customer.FirstNameRequired";
        public static readonly string USERID_ALREADY_EXISTS = "Customer.UserIdAlreadyExists";
        public static readonly string NOTFOUND = "Customer.NotFound";
        public static readonly string NOTFOUND_BY_USER = "Customer.NotFoundByUser";
        public static readonly string ADDRESS_ALREADY_EXISTS = "Customer.AddressAlreadyExists";
    }

    public static class Address
    {
        public static readonly string ID_REQUIRED = "Address.IdRequired";
        public static readonly string LINE1_REQUIRED = "Address.Line1Required";
        public static readonly string LINE1_LENGTH = "Address.Line1Length";
        public static readonly string LINE2_LENGTH = "Address.Line2Length";
        public static readonly string LINE2_REQUIRED = "Address.Line2Required";
        public static readonly string CITY_LENGTH = "Address.CityLength";
        public static readonly string CITY_REQUIRED = "Address.CityRequired";
        public static readonly string STATE_REQUIRED = "Address.StateRequired";
        public static readonly string STATE_LENGTH = "Address.StateLength";
        public static readonly string POSTALCODE_LENGTH = "Address.PostalCodeLength";
        public static readonly string POSTALCODE_REQUIRED = "Address.PostalCodeRequired";
        public static readonly string COUNTRY_LENGTH = "Address.CountryLength";
        public static readonly string COUNTRY_REQUIRED = "Address.CountryRequired";
        public static readonly string NOTFOUND = "Address.NotFound";

    }

    public static class PhoneNumber
    {
        public static readonly string INVALID_FORMAT = "PhoneNumber.InvalidFormat";
        public static readonly string IS_REQUIRED = "PhoneNumber.IsRequired";
    }
    public static class FirstName
    {
        public static readonly string IS_REQUIRED = "FirstName.IsRequired";
        public static readonly string MIN_LENGTH = "FirstName.MinLength";
        public static readonly string MAX_LENGTH = "FirstName.MaxLength";
        public static readonly string IS_ONLY_LETTERS = "FirstName.IsOnlyLetters";
    }

    public static class LastName
    {
        public static readonly string IS_REQUIRED = "LastName.IsRequired";
        public static readonly string MIN_LENGTH = "LastName.MinLength";
        public static readonly string MAX_LENGTH = "LastName.MaxLength";
        public static readonly string IS_ONLY_LETTERS = "FirstName.IsOnlyLetters";
    }
}