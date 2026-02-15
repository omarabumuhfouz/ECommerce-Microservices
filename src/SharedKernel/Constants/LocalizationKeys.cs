namespace SharedKernel.Constants;

public static class LocalizationKeys
{
    public static class Orders
    {
        public const string CREATED = "";
        public const string RETRIEVED = "";
        public const string UPDATED = "";
        public const string DELETED = "";
        public const string NOT_FOUND = "";
        public const string INVALID_STATUS = "";
        public const string INVALID_STATUS_TRANSITION = "";
    }

    public static class Customers
    {
        public const string ADDED = "Customers:ADDED";
        public const string UPDATED = "Customers:UPDATED";
        public const string NOT_FOUND = "Customers:NOT_FOUND";
        public const string NOT_FOUND_BY_USER = "Customers:NOT_FOUND_BY_USER";
        public const string HAS_NO_ADDRESSES = "Customers:HAS_NO_ADDRESSES";
        public const string USER_ALREADY_EXISTS = "Customers:USER_ALREADY_EXISTS";
        public const string USER_ID_REQUIRED = "Customers:USER_ID_REQUIRED";
        public const string FIRST_NAME_REQUIRED = "Customers:FIRST_NAME_REQUIRED";
        public const string FIRST_NAME_LENGTH = "Customers:FIRST_NAME_LENGTH";
        public const string FIRST_NAME_LETTERS_ONLY = "Customers:FIRST_NAME_LETTERS_ONLY";
        public const string LAST_NAME_REQUIRED = "Customers:LAST_NAME_REQUIRED";
        public const string LAST_NAME_LENGTH = "Customers:LAST_NAME_LENGTH";
        public const string LAST_NAME_LETTERS_ONLY = "Customers:LAST_NAME_LETTERS_ONLY";
        public const string PHONE_NUMBER_REQUIRED = "Customers:PHONE_NUMBER_REQUIRED";
        public const string PHONE_NUMBER_INVALID = "Customers:PHONE_NUMBER_INVALID";
    }

    public static class Products
    {
        public const string NOT_FOUND = "PRODUCT_NOT_FOUND";
        public const string INSUFFICIENT_STOCK = "INSUFFICIENT_STOCK";
    }

    public static class Addresses
    {
        public const string DELETED = "Addresses:DELETED";
        public const string ADDED = "Addresses:ADDED";
        public const string UPDATED = "Addresses:UPDATED";
        public const string NOT_FOUND = "Addresses:NOT_FOUND";
        public const string NOT_BELONG_TO_CUSTOMER = "Addresses:NOT_BELONG_TO_CUSTOMER";
        public const string SET_DEFAULT_SUCCESS = "Addresses:SET_DEFAULT_SUCCESS";
        public const string ADDRESS_LINE1_REQUIRED = "Addresses:ADDRESS_LINE1_REQUIRED";
        public const string ADDRESS_LINE1_MAX_LENGTH = "Addresses:ADDRESS_LINE1_MAX_LENGTH";
        public const string ADDRESS_LINE2_MAX_LENGTH = "Addresses:ADDRESS_LINE2_MAX_LENGTH";
        public const string CITY_REQUIRED = "Addresses:CITY_REQUIRED";
        public const string CITY_MAX_LENGTH = "Addresses:CITY_MAX_LENGTH";
        public const string STATE_MAX_LENGTH = "Addresses:STATE_MAX_LENGTH";
        public const string POSTAL_CODE_REQUIRED = "Addresses:POSTAL_CODE_REQUIRED";
        public const string POSTAL_CODE_MAX_LENGTH = "Addresses:POSTAL_CODE_MAX_LENGTH";
        public const string COUNTRY_REQUIRED = "Addresses:COUNTRY_REQUIRED";
        public const string COUNTRY_MAX_LENGTH = "Addresses:COUNTRY_MAX_LENGTH";
        public const string CUSTOMER_ID_REQUIRED = "Addresses:CUSTOMER_ID_REQUIRED";
        public const string ADDRESS_ID_REQUIRED = "Addresses:ADDRESS_ID_REQUIRED";
    }

    public static class Operations
    {
        public const string SUCCESS = "Operations:SUCCESS";
        public const string FAILED = "Operations:FAILED";
        public const string NOT_FOUND = "Operations:NOT_FOUND";
    }

    public static class Validations
    {
        public const string VALIDATION_ERROR_OCCURRED = "Validations:VALIDATION_ERROR_OCCURRED";
        public const string ARGUMENT_OUT_OF_RANGE = "Validations:ARGUMENT_OUT_OF_RANGE";
        public const string INVALID_OPERATION = "Validations:INVALID_OPERATION";
        public const string REQUIRED_PARAMETER_NULL = "Validations:REQUIRED_PARAMETER_NULL";
        public const string UNEXPECTED_ERROR = "Validations:UNEXPECTED_ERROR";
    }

}
