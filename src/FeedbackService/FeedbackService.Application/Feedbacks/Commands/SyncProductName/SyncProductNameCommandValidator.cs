namespace FeedbackService.Application.Feedbacks.Commands.SyncProductName;

public class SyncProductNameCommandValidator : AbstractValidator<SyncProductNameCommand>
{
    public SyncProductNameCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("ProductId is required for the name synchronization.");

        RuleFor(x => x.NewName)
            .NotEmpty()
            .WithMessage("Product name cannot be empty.")
            .MaximumLength(FeedbackConstants.MaxProductNameLength) // Adjust based on your Product Name constraints
            .WithMessage("Product name must not exceed 200 characters.");

        // Prevent updating with just spaces
        RuleFor(x => x.NewName)
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Product name must contain valid characters.");
    }
}