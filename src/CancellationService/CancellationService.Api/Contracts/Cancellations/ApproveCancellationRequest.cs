namespace CancellationService.Api.Contracts.Cancellations;

public record ApproveCancellationRequest(string Remarks, decimal Charges);