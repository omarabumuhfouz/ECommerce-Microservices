namespace CustomerService.Domain.Constants;

public static class CustomerConstants
{
    public const int MIN_FIRST_NAME_LENGTH = 2;
    public const int MAX_FIRST_NAME_LENGTH = 50;

    public const int MIN_LAST_NAME_LENGTH = 2;
    public const int MAX_LAST_NAME_LENGTH = 50;

    public const int MIN_PHONE_NUMBER_LENGTH = 8;
    public const int MAX_PHONE_NUMBER_LENGTH = 15;
    

    public const string SEARCH_BY_FIRST_NAME = "firstname";
    public const string SEARCH_BY_LAST_NAME = "lastname";
    public const string SEARCH_BY_PHONE_NUMBER = "phonenumber";

}