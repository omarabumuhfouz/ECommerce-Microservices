namespace CancellationService.Application.Cancellations.Queries.GetCancellationById;

public class GetCancellationByIdQueryValidator : AbstractValidator<GetCancellationByIdQuery>
{
    public GetCancellationByIdQueryValidator()
    {
        RuleFor(x => x.CancellationId)
            .ValidateCancellationId();
    }
}