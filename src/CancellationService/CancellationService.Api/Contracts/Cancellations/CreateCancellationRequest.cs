namespace CancellationService.Api.Contracts.Cancellations;
public record CreateCancellationRequest(Guid OrderId, string Reason);