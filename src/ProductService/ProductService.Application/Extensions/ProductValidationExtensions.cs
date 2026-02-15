using FluentValidation;
using ProductService.Domain.Constants;
using ProductService.Domain.Errors;

namespace ProductService.Application.Extensions;

public static class ProductValidationExtensions
{
    public static IRuleBuilderOptions<T, Guid> ValidateProductId<T>(
        this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Product.IdRequired);
    }

    public static IRuleBuilderOptions<T, string> ValidateProductName<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Product.NameRequired)
            .Length(ProductConstants.NAME_MIN_LENGTH, ProductConstants.NAME_MAX_LENGTH)
                .WithErrorCode(ErrorCodes.Product.NameLength)
                .WithMetadata(ErrorMetadataKeys.MinLength, ProductConstants.NAME_MIN_LENGTH)
                .WithMetadata(ErrorMetadataKeys.MaxLength, ProductConstants.NAME_MAX_LENGTH);
    }

    public static IRuleBuilderOptions<T, string> ValidateProductDescription<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Product.DescriptionRequired)
            .MinimumLength(ProductConstants.DESCRIPTION_MIN_LENGTH)
                .WithErrorCode(ErrorCodes.Product.DescriptionMinLength) // Assuming you mapped this
                .WithMetadata(ErrorMetadataKeys.MinLength, ProductConstants.DESCRIPTION_MIN_LENGTH);
    }

    public static IRuleBuilderOptions<T, decimal> ValidatePrice<T>(
        this IRuleBuilder<T, decimal> ruleBuilder)
    {
        return ruleBuilder
            .InclusiveBetween(ProductConstants.PRICE_MIN, ProductConstants.PRICE_MAX)
                .WithErrorCode(ErrorCodes.Product.PriceRange)
                .WithMetadata(ErrorMetadataKeys.MinValue, ProductConstants.PRICE_MIN)
                .WithMetadata(ErrorMetadataKeys.MaxValue, ProductConstants.PRICE_MAX);
    }

    public static IRuleBuilderOptions<T, string> ValidateCurrency<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Product.CurrencyRequired)
            .Length(3)
                .WithErrorCode(ErrorCodes.Product.CurrencyLength)
                .WithMetadata(ErrorMetadataKeys.MaxLength, 3);
    }

    public static IRuleBuilderOptions<T, int> ValidateStockQuantity<T>(
        this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder
            .InclusiveBetween(ProductConstants.STOCK_MIN, ProductConstants.STOCK_MAX)
                .WithErrorCode(ErrorCodes.Product.StockRange)
                .WithMetadata(ErrorMetadataKeys.MinValue, ProductConstants.STOCK_MIN)
                .WithMetadata(ErrorMetadataKeys.MaxValue, ProductConstants.STOCK_MAX);
    }

    public static IRuleBuilderOptions<T, int> ValidateDiscountPercentage<T>(
        this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder
            .InclusiveBetween(ProductConstants.DISCOUNT_MIN, ProductConstants.DISCOUNT_MAX)
                .WithErrorCode(ErrorCodes.Product.DiscountRange)
                .WithMetadata(ErrorMetadataKeys.MinValue, ProductConstants.DISCOUNT_MIN)
                .WithMetadata(ErrorMetadataKeys.MaxValue, ProductConstants.DISCOUNT_MAX);
    }

    public static IRuleBuilderOptions<T, DateTime> ValidateDiscountEndDate<T>(
        this IRuleBuilder<T, DateTime> ruleBuilder)
    {
        return ruleBuilder
            .Must(date => date > DateTime.UtcNow)
                .WithErrorCode(ErrorCodes.Product.DiscountDateFuture);
    }

    // --- Feature Validations ---

    public static IRuleBuilderOptions<T, string> ValidateFeatureName<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Feature.NameRequired)
            .MaximumLength(ProductConstants.FEATURE_NAME_MAX_LENGTH)
                .WithErrorCode(ErrorCodes.Feature.NameLength)
                .WithMetadata(ErrorMetadataKeys.MaxLength, ProductConstants.FEATURE_NAME_MAX_LENGTH);
    }

    public static IRuleBuilderOptions<T, string> ValidateFeatureValue<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Feature.ValueRequired)
            .MaximumLength(ProductConstants.FEATURE_VALUE_MAX_LENGTH)
                .WithErrorCode(ErrorCodes.Feature.ValueLength)
                .WithMetadata(ErrorMetadataKeys.MaxLength, ProductConstants.FEATURE_VALUE_MAX_LENGTH);
    }

    // --- Image Validations ---

    public static IRuleBuilderOptions<T, string> ValidateImageUrl<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Image.UrlRequired)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .WithErrorCode(ErrorCodes.Image.UrlInvalid);
    }

    public static IRuleBuilderOptions<T, string?> ValidateImageAltText<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(ProductConstants.IMAGE_ALT_TEXT_MAX_LENGTH)
                .WithErrorCode(ErrorCodes.Image.AltTextLength)
                .WithMetadata(ErrorMetadataKeys.MaxLength, ProductConstants.IMAGE_ALT_TEXT_MAX_LENGTH);
    }

    // --- Tag Validations ---

    public static IRuleBuilderOptions<T, Guid> ValidateTagId<T>(
        this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Tag.IdRequired);
    }

    public static IRuleBuilderOptions<T, string> ValidateTagName<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Tag.NameRequired)
            .MaximumLength(50)
                .WithErrorCode(ErrorCodes.Tag.NameLength)
                .WithMetadata(ErrorMetadataKeys.MaxLength, 50);
    }
}
