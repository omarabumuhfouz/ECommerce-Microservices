using FluentValidation;
using ProductService.Domain.Constants;
using ProductService.Domain.Errors;

namespace ProductService.Application.Extensions;

public static class CategoryValidationExtensions
{
    public static IRuleBuilderOptions<T, Guid> ValidateCategoryId<T>(
        this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Category.IdRequired);
    }

    public static IRuleBuilderOptions<T, string> ValidateCategoryName<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Category.NameRequired)
            .MaximumLength(CategoryConstants.NAME_MAX_LENGTH)
                .WithErrorCode(ErrorCodes.Category.NameLength)
                .WithMetadata(ErrorMetadataKeys.MaxLength, CategoryConstants.NAME_MAX_LENGTH);
    }

    public static IRuleBuilderOptions<T, string> ValidateCategoryDescription<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Category.DescriptionRequired)
            .MaximumLength(CategoryConstants.DESCRIPTION_MAX_LENGTH) // Assuming you have this in CategoryConstants
                .WithErrorCode(ErrorCodes.Category.DescriptionLength)
                .WithMetadata(ErrorMetadataKeys.MaxLength, CategoryConstants.DESCRIPTION_MAX_LENGTH);
    }
}
