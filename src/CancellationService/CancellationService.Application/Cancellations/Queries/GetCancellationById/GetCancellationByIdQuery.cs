namespace CancellationService.Application.Cancellations.Queries.GetCancellationById;

public record GetCancellationByIdQuery(Guid CancellationId) : IQuery<CancellationDto>;
