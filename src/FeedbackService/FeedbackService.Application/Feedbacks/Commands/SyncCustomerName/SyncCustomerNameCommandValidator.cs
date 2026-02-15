using FeedbackService.Application.Common.Extensions;

namespace FeedbackService.Application.Feedbacks.Commands.SyncCustomerName;

public class SyncCustomerNameCommandValidator : AbstractValidator<SyncCustomerNameCommand>
{
    public SyncCustomerNameCommandValidator()
    {
        RuleFor(x => x.CustomerId)
        .ValidateCustomerId();

        RuleFor(x => x.NewName)
            .NotEmpty()
            .WithMessage("The new customer name cannot be empty.")
            .MaximumLength(FeedbackConstants.MaxCustomerNameLength)
            .WithMessage("The customer name is too long.");
    }
}