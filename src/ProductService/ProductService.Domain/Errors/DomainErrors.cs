using ProductService.Domain.Constants;
using SharedKernel.Shared; // Assuming Error lives here
using SharedKernel.Primitives.Result; // Or wherever Error lives

namespace ProductService.Domain.Errors;

public static class DomainErrors
{
    public static class Product
    {
        public static readonly Func<Guid, Error> NotFound = id => Error.NotFound(
            ErrorCodes.Product.NotFound,
            $"The product with the identifier {id} was not found.");

        public static readonly Func<List<Guid>, Error> MissingIds = ids => Error.NotFound(
            ErrorCodes.Product.MissingIds,
            $"Product with IDs [{string.Join(", ", ids)}] were not found."
        );

        public static readonly Error NameRequired = Error.Validation(
            ErrorCodes.Product.NameRequired,
            "The product name is required.");

        public static readonly Error DescriptionRequired = Error.Validation(
            ErrorCodes.Product.DescriptionRequired,
            "The product description is required.");

        public static readonly Error QuantityMustBePositive = Error.Validation(
            ErrorCodes.Product.QuantityMustBePositive,
            "The product quantity must be a positive number.");

        public static readonly Error InsufficientStock = Error.Conflict(
            ErrorCodes.Product.InsufficientStock,
            "The product does not have sufficient stock for the requested operation.");

        public static readonly Func<string, Error> DuplicateName = name => Error.Conflict(
            ErrorCodes.Product.DuplicateName,
            $"A product with the name '{name}' already exists.");

        public static readonly Func<string, Error> InvalidStockOperation = operation => Error.Validation(
            ErrorCodes.Product.InvalidStockOperation,
            $"Invalid stock operation: '{operation}'. Must be 'increase' or 'decrease'."
        );
    }

    public static class Category
    {
        public static readonly Error IdRequired = Error.Validation(
            ErrorCodes.Category.IdRequired,
            "The category identifier is required.");

        public static readonly Error NameRequired = Error.Validation(
            ErrorCodes.Category.NameRequired,
            "The category name is required.");

        public static readonly Error DescriptionRequired = Error.Validation(
            ErrorCodes.Category.DescriptionRequired,
            "The category description is required.");

        public static readonly Error NameTooLong = Error.Validation(
            ErrorCodes.Category.NameTooLong,
            $"Category name is too long must be less than {CategoryConstants.NAME_MAX_LENGTH}");

        public static readonly Func<string, Error> DuplicateName = name => Error.Conflict(
            ErrorCodes.Category.DuplicateName,
            $"Category with name : {name}, already exists.");

        public static readonly Func<Guid, Error> NotFound = id => Error.NotFound(
            ErrorCodes.Category.NotFound,
            $"Category with id : {id}, was not found.");

        public static readonly Func<Guid, Error> HasProducts = id => Error.Conflict(
            ErrorCodes.Category.HasProducts,
            $"Category with id : {id}, has products and cannot be deleted.");
    }

    public static class Tag
    {
        public static readonly Error IdRequired = Error.Validation(
            ErrorCodes.Tag.IdRequired,
            "Tag Id is Required.");

        public static readonly Error NameRequired = Error.Validation(
            ErrorCodes.Tag.NameRequired,
            "Tag Name is Required.");

        public static readonly Func<Guid, Error> NotFound = id => Error.NotFound(
            ErrorCodes.Tag.NotFound,
            $"Tag with Id : {id}, was not found.");

        public static readonly Func<List<Guid>, Error> NotFoundList = ids => Error.NotFound(
            ErrorCodes.Tag.NotFound,
            $"Tags with IDs [{string.Join(", ", ids)}] were not found.");

        public static readonly Func<Guid, Error> AlreadyExists = id => Error.Conflict(
            ErrorCodes.Tag.AlreadyExists,
            $"Tag with Id :{id}, already exists in product");

        public static readonly Error HasAssociatedProducts = Error.Conflict(
            ErrorCodes.Tag.HasAssociatedProducts,
            "The tag cannot be deleted because it is associated with existing products.");

        public static readonly Func<string, Error> NameAlreadyExists = name => Error.Conflict(
            ErrorCodes.Tag.NameAlreadyExists,
            $"A tag with the name: {name} already exists."
        );
    }

    public static class Discount
    {
        public static readonly Error InvalidPercentage = Error.Validation(
            ErrorCodes.Discount.InvalidPercentage,
            $"The discount percentage must be between {ProductConstants.DISCOUNT_MIN} and {ProductConstants.DISCOUNT_MAX}.");

        public static readonly Error InvalidEndDate = Error.Validation(
            ErrorCodes.Discount.InvalidEndDate,
            "The discount end date cannot be in the past.");

        public static readonly Error AlreadyExists = Error.Conflict(
            ErrorCodes.Discount.AlreadyExists,
            "Product already has a Discount on it.");

        public static readonly Error NotFound = Error.NotFound(
            ErrorCodes.Discount.NotFound,
            "No discount exists for this product."
        );
    }

    public static class Feature
    {
        public static readonly Error NameRequired = Error.Validation(
            ErrorCodes.Feature.NameRequired,
            "The feature name is required.");

        public static readonly Error ValueRequired = Error.Validation(
            ErrorCodes.Feature.ValueRequired,
            "The feature value is required.");

        public static readonly Error NameTooLong = Error.Validation(
            ErrorCodes.Feature.NameTooLong,
            $"Feature name is too long must be less than {ProductConstants.FEATURE_NAME_MAX_LENGTH} characters.");

        public static readonly Error ValueTooLong = Error.Validation(
            ErrorCodes.Feature.ValueTooLong,
            $"Feature value is too long must be less than {ProductConstants.FEATURE_VALUE_MAX_LENGTH} characters.");

        public static readonly Func<string, Error> AlreadyExists = name => Error.Conflict(
            ErrorCodes.Feature.AlreadyExists,
            $"Feature with name '{name}' already exists or is duplicated in the request."
        );

        public static readonly Func<string, Error> NotFound = name => Error.NotFound(
            ErrorCodes.Feature.NotFound,
            $"The Feature with name : {name}, was not found.");
    }

    public static class Image
    {
        public static readonly Error UrlRequired = Error.Validation(
            ErrorCodes.Image.UrlRequired,
            "The image URL is required.");

        public static readonly Error ListNull = Error.Validation(
            ErrorCodes.Image.ListNull,
            "The list of images cannot be null.");

        public static readonly Error ItemNull = Error.Validation(
            ErrorCodes.Image.ItemNull,
            "The list of images to add cannot contain null entries.");

        public static readonly Func<string, Error> DuplicateUrl = url => Error.Conflict(
            ErrorCodes.Image.DuplicateUrl,
            $"An image with the URL '{url}' is already associated with this product.");

        public static readonly Func<string, Error> NotFound = url => Error.NotFound(
            ErrorCodes.Image.NotFound,
            $"The image with Url :{url}, was not found");

        public static readonly Func<string, Error> AlreadyExists = url => Error.Conflict(
            ErrorCodes.Image.AlreadyExists,
            $"The image with Url: {url}, Already Exists in this product.");
    }

    public static class Money
    {
        public static readonly Error InvalidAmount = Error.Validation(
            ErrorCodes.Money.InvalidAmount,
            "The money amount cannot be negative.");

        public static readonly Error CurrencyRequired = Error.Validation(
            ErrorCodes.Money.CurrencyRequired,
            "The money currency is required.");
    }
}
