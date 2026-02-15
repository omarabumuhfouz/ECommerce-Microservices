using System.Text.Json.Serialization;

namespace CancellationService.Domain.Cancellations.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CancellationStatus
{
    Pending = 1,
    Approved = 3,
    Rejected = 4
}