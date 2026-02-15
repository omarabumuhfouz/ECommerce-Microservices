namespace AuthService.Application.Features.Users.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.User.IdRequired)
            .WithMessage("User ID is required.");

        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.User.CurrentPasswordRequired)
            .WithMessage("Current password is required.");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.User.NewPasswordRequired)
            .WithMessage("New password is required.")
            .MinimumLength(8)
            .WithErrorCode(ErrorCodes.User.NewPasswordLength)
            .WithMessage("New password must be at least 8 characters long.")
            .MaximumLength(100)
            .WithErrorCode(ErrorCodes.User.NewPasswordLength)
            .Matches(@"[A-Z]").WithErrorCode(ErrorCodes.User.NewPasswordFormat).WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[0-9]").WithErrorCode(ErrorCodes.User.NewPasswordFormat).WithMessage("Password must contain at least one number.");

        RuleFor(x => x.ConfirmNewPassword)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.User.ConfirmPasswordRequired)
            .Equal(x => x.NewPassword)
            .WithErrorCode(ErrorCodes.User.PasswordsDoNotMatch)
            .WithMessage("The new password and confirmation password do not match.");
    }
}