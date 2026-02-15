using CancellationService.Domain.Cancellations.Enums;

namespace CancellationService.Application.Cancellations.DTOs;
public class CancellationDto
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public string Reason { get; set; } = string.Empty;
    public CancellationStatus Status { get; set; }
    public DateTime RequestedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public Guid? ProcessedBy { get; set; }
    public decimal OrderAmount { get; set; }
    public decimal? CancellationCharges { get; set; }
    public string? Remarks { get; set; }
}