namespace CustomerService.Application.Extensions;

internal static class CustomerValidationExtensions
{
    public static IRuleBuilderOptions<T, Guid> ValidateUserId<T>(this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Customer.USERID_REQUIRED);
    }
    
    public static IRuleBuilderOptions<T, string> ValidateFirstName<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
                .WithErrorCode(ErrorCodes.FirstName.IS_REQUIRED)
            .MinimumLength(CustomerConstants.MIN_FIRST_NAME_LENGTH)
                 .WithErrorCode(ErrorCodes.FirstName.MIN_LENGTH)
                 .WithMetadata(ErrorMetadataKeys.MIN_LENGTH, CustomerConstants.MAX_FIRST_NAME_LENGTH)
            .MaximumLength(CustomerConstants.MAX_FIRST_NAME_LENGTH)
                 .WithErrorCode(ErrorCodes.FirstName.MAX_LENGTH)
                 .WithMetadata(ErrorMetadataKeys.MAX_LENGTH, CustomerConstants.MAX_FIRST_NAME_LENGTH)
            .Matches("^[A-Za-z]+$")
                 .WithErrorCode(ErrorCodes.FirstName.IS_ONLY_LETTERS);
    }

    public static IRuleBuilderOptions<T, string> ValidateLastName<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
                 .WithErrorCode(ErrorCodes.LastName.IS_REQUIRED)
            .MinimumLength(CustomerConstants.MIN_LAST_NAME_LENGTH)
                 .WithErrorCode(ErrorCodes.LastName.MIN_LENGTH)
                 .WithMetadata(ErrorMetadataKeys.MIN_LENGTH, CustomerConstants.MIN_LAST_NAME_LENGTH)
            .MaximumLength(CustomerConstants.MAX_LAST_NAME_LENGTH)
                 .WithErrorCode(ErrorCodes.LastName.MAX_LENGTH)
                 .WithMetadata(ErrorMetadataKeys.MAX_LENGTH, CustomerConstants.MAX_LAST_NAME_LENGTH)
            .Matches("^[A-Za-z]+$")
                 .WithErrorCode(ErrorCodes.LastName.IS_ONLY_LETTERS);
    }

    public static IRuleBuilderOptions<T, string> ValidatePhoneNumber<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
                .WithErrorCode(ErrorCodes.PhoneNumber.IS_REQUIRED);
            // .Matches(@$"^\+?[0-9]{CustomerConstants.MinPhoneNumberLength,CustomerConstants.MaxPhoneNumberLength}$")
            // .WithMessage(localizer[LocalizationKeys.Validation.PhoneNumberInvalid, CustomerConstants.MinPhoneNumberLength, CustomerConstants.MaxPhoneNumberLength]);
    }

    public static IRuleBuilderOptions<T, Guid> ValidateCustomerId<T>(
            this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Customer.ID_REQUIRED);
    }

    public static IRuleBuilderOptions<T, Guid> ValidateAddressId<T>(
            this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Address.ID_REQUIRED);
    }

    public static IRuleBuilderOptions<T, string> ValidateAddressLine1<T>(
                this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Address.LINE1_REQUIRED)
            .MaximumLength(AddressConstants.ADDRESS_LINE1_MAX_LENGTH)
                .WithErrorCode(ErrorCodes.Address.LINE1_LENGTH)
                .WithMetadata(ErrorMetadataKeys.MAX_LENGTH, AddressConstants.ADDRESS_LINE1_MAX_LENGTH);
    }

    public static IRuleBuilderOptions<T, string> ValidateAddressLine2<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(AddressConstants.ADDRESS_LINE1_MAX_LENGTH)
                .WithErrorCode(ErrorCodes.Address.LINE2_LENGTH)
                .WithMetadata(ErrorMetadataKeys.MAX_LENGTH, AddressConstants.ADDRESS_LINE2_MAX_LENGTH);
    }

    public static IRuleBuilderOptions<T, string> ValidateCity<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Address.CITY_REQUIRED)
            .MaximumLength(AddressConstants.CITY_MAX_LENGTH)
                .WithErrorCode(ErrorCodes.Address.CITY_LENGTH)
                .WithMetadata(ErrorMetadataKeys.MIN_LENGTH, AddressConstants.CITY_MAX_LENGTH);
    }

    public static IRuleBuilderOptions<T, string> ValidateState<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(AddressConstants.STATE_MAX_LENGTH)
                .WithErrorCode(ErrorCodes.Address.STATE_LENGTH)
                .WithMetadata(ErrorMetadataKeys.MAX_LENGTH, AddressConstants.STATE_MAX_LENGTH);

    }

        public static IRuleBuilderOptions<T, string> ValidatePostalCode<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
        return ruleBuilder
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Address.POSTALCODE_REQUIRED)
            .MaximumLength(AddressConstants.POSTAL_CODE_MAX_LENGTH)
                    .WithErrorCode(ErrorCodes.Address.POSTALCODE_LENGTH)
                    .WithMetadata(ErrorMetadataKeys.MAX_LENGTH, AddressConstants.POSTAL_CODE_MAX_LENGTH);
        }

        public static IRuleBuilderOptions<T, string> ValidateCountry<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
        return ruleBuilder
            .NotEmpty()
                .WithErrorCode(ErrorCodes.Address.COUNTRY_REQUIRED)
            .MaximumLength(AddressConstants.COUNTRY_MAX_LENGTH)
                .WithErrorCode(ErrorCodes.Address.COUNTRY_LENGTH)
                .WithMetadata(ErrorMetadataKeys.MAX_LENGTH, AddressConstants.COUNTRY_MAX_LENGTH);
        }
}
