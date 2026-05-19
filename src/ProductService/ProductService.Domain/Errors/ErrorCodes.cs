namespace ProductService.Domain.Errors;

public static class ErrorCodes
{
    public static class Product
    {
        public const string NotFound = "Product.NotFound";
        public const string MissingIds = "Product.MissingIds";
        public const string EmptyName = "Product.NameRequired";
        public const string DescriptionRequired = "Product.DescriptionRequired";
        public const string QuantityMustBePositive = "Product.QuantityMustBePositive"; 
        public const string DuplicateName = "Product.DuplicateName";
        public const string InvalidPrice = "Product.InvalidPrice";



        public const string IdRequired = "Product.IdRequired";
        public const string NameLength = "Product.NameLength";
        public const string DescriptionMinLength = "Product.DescriptionMinLength";
        public const string PriceRange = "Product.PriceRange";
        public const string CurrencyRequired = "Product.CurrencyRequired";
        public const string CurrencyLength = "Product.CurrencyLength";
        public const string StockRange = "Product.StockRange";
        public const string DiscountRange = "Product.DiscountRange";
        public const string DiscountDateFuture = "Product.DiscountDateFuture";
        
        // These were used in your validators but missing from your snippet
        public const string TagIdRequired = "Product.TagIdRequired";
        public const string TagNameRequired = "Product.TagNameRequired";
        public const string FeatureNameRequired = "Product.FeatureNameRequired";
        public const string FeatureNameLength = "Product.FeatureNameLength";
        public const string FeatureValueRequired = "Product.FeatureValueRequired";
        public const string FeatureValueLength = "Product.FeatureValueLength";
        public const string ImageUrlRequired = "Product.ImageUrlRequired";
        public const string ImageUrlInvalid = "Product.ImageUrlInvalid";
        public const string ImageAltLength = "Product.ImageAltLength";
    }

    public static class Category
    {
        public const string IdRequired = "Category.IdRequired";
        public const string NameRequired = "Category.NameRequired";
        public const string DescriptionRequired = "Category.DescriptionRequired";
        public const string NameTooLong = "Category.NameTooLong";
        public const string DuplicateName = "Category.DuplicateName";
        public const string NotFound = "Category.NotFound";
        public const string HasProducts = "Category.HasProducts";

        public const string NameLength = "Category.NameLength";
        public const string DescriptionLength = "Category.DescriptionLength";
    }

    public static class Tag
    {
        public const string IdRequired = "Tag.IdRequired"; // Fixed typo "Requried" in key
        public const string NameRequired = "Tag.NameRequired";
        public const string AlreadyExists = "Tag.AlreadyExists";
        public const string NotFound = "Tag.NotFound";
        public const string HasAssociatedProducts = "Tag.HasAssociatedProducts";
        public const string NameAlreadyExists = "Tag.NameAlreadyExists";

        public const string NameLength = "Tag.NameLength";
    }

    public static class Discount
    {
        public const string InvalidPercentage = "Discount.InvalidPercentage";
        public const string ExpiredDiscount = "Discount.ExpiredDiscount";
        public const string InvalidEndDate = "Discount.InvalidEndDate";
        public const string InvalidStartDate = "Discount.InvalidStartDate";
        public const string NotFound = "Discount.NotFound";
        public const string AlreadyExists = "Discount.AlreadyExists";
    }

    public static class Feature
    {
        public const string NameRequired = "Feature.NameRequired";
        public const string ValueRequired = "Feature.ValueRequired";
        public const string NameTooLong = "Feature.NameTooLong";
        public const string ValueTooLong = "Feature.ValueTooLong";
        public const string AlreadyExists = "Feature.AlreadyExists";
        public const string NotFound = "Feature.NotFound";

        public const string NameLength = "Feature.NameLength";
        public const string ValueLength = "Feature.ValueLength";
    }

    public static class Image
    {
        public const string UrlRequired = "Image.UrlRequired";
        public const string ListNull = "Image.ListNull";
        public const string ItemNull = "Image.ItemNull";
        public const string DuplicateUrl = "Image.DuplicateUrl";
        public const string CreationFailed = "Image.CreationFailed";
        public const string NotFound = "Image.NotFound";
        public const string AlreadyExists = "Image.AlreadyExists";

        public const string UrlInvalid = "Image.UrlInvalid";
        public const string AltTextLength = "Image.AltTextLength";
    }

    public static class Money
    {
        public const string InvalidAmount = "Money.InvalidAmount";
        public const string CurrencyRequired = "Money.CurrencyRequired";
    }
}