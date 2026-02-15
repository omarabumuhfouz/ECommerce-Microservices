using System.Text.Json.Serialization;

namespace PaymentService.Application.Common.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatus
{
    Pending = 1,
    Processing,
    Shipped,
    Delivered,
    Canceled,
    Refunded
}