namespace CancellationService.Application.Cancellations.Queries.GetCancellationByOrderId;

public record GetCancellationByOrderIdQuery(Guid OrderId) : ICommand<CancellationDto>;