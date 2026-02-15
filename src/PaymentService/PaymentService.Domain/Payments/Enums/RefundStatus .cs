using System.Text.Json.Serialization;
namespace PaymentService.Domain.Payments.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RefundStatus
{
    Pending = 1,
    Completed,
    Failed
}