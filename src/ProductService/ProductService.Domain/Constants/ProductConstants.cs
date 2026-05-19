namespace ProductService.Domain.Constants;

public static class ProductConstants
{
    public const int NameMaxLength = 100;
    public const int NameMinLength = 3;
    public const int DescriptionMaxLength = 500;
    public const int DescriptionMinLength = 0;
    public const int CurrencyMaxLength = 3;
    public const int DiscountMaxPercentage = 100;
    public const int DiscountMinPercentage = 0;
    public const int ImageUrlMaxLength = 500;
    public const int ImageAltTextMaxLength = 200;
    public const decimal PriceMin = 0.1m;
    public const decimal PriceMax = 10_000m;
    public const int DiscountMax = 100;
    public const int DiscountMin = 0;

    public const int FeatureNameMaxLength = 50;
    public const int FeatureValueMaxLength = 200;
    public const int MaxAddedFeaturesAtOnce = 20;
    public const int MaxAddedImagesAtOnce = 10;

    public const string SearchByName = "name";
    public const string SearchByPrice = "PRICE";
}
