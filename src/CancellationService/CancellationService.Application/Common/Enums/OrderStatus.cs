using System.Text.Json.Serialization;

namespace CancellationService.Application.Enums
{
    // Enum to represent the status of an order
    // The JsonStringEnumConverter ensures that enums are serialized as their string names
    // instead of integer values, enhancing readability.
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderStatus
    {
        Pending = 1,
        Processing,
        Shipped,
        Delivered,
        Refunded,
        Canceled
    }
}