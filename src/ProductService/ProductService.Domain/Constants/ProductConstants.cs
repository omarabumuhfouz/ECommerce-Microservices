namespace ProductService.Domain.Constants;


public static class ProductConstants
{
    public const int NAME_MAX_LENGTH = 100;
    public const int NAME_MIN_LENGTH = 3;
    public const int DESCRIPTION_MAX_LENGTH = 1000;
    public const int DESCRIPTION_MIN_LENGTH = 0;
    public const int CURRENCY_MAX_LENGTH = 3;
    public const int DISCOUNT_MAX_PERCENTAGE = 100;
    public const int DISCOUNT_MIN_PERCENTAGE = 0;
    public const int IMAGE_URL_MAX_LENGTH = 500;
    public const int IMAGE_ALT_TEXT_MAX_LENGTH = 200;
    public const decimal PRICE_MIN = 0.1m;
    public const decimal PRICE_MAX = 10_000m;
    public const int STOCK_MAX = 1_000_000;
    public const int STOCK_MIN = 1;
    public const int DISCOUNT_MAX = 100;
    public const int DISCOUNT_MIN = 0;

    public const int FEATURE_NAME_MAX_LENGTH = 50;
    public const int FEATURE_VALUE_MAX_LENGTH = 200;
    public const int MAX_ADDED_FEATURES_AT_ONCE = 20;
    public const int MAX_ADDED_IMAGES_AT_ONCE = 10;

    public const string INCREASE = "increase";
    public const string DECREASE = "decrease";
    public const string SEARCH_BY_NAME = "name";
    public const string SEARCH_BY_PRICE = "PRICE";
}

