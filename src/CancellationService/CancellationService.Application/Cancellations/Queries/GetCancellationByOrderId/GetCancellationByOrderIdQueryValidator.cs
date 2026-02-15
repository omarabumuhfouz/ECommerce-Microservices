using FluentValidation;

namespace CancellationService.Application.Cancellations.Queries.GetCancellationByOrderId;

public class GetCancellationByOrderIdQueryValidator : AbstractValidator<GetCancellationByOrderIdQuery>
{
    public GetCancellationByOrderIdQueryValidator()
    {
        RuleFor(x => x.OrderId).ValidateAdminId()
            .NotEmpty()
            .WithMessage("Order ID is required.");
    }
}