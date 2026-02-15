using System.Text.Json;
using System.Text.Json.Serialization;
using OrderService.Domain.Entities;
using OrderService.Tests.DTOs;

public class TestDataLoader
{
    public class TestData
    {
        public List<OrderTestDto> Orders { get; set; }
    }

    public static List<Order> LoadData(IMapper mapper)
    {

        var filePath = "/home/omar/Downloads/MicroservicesProjects/ECommerce-Microservices/src/OrderService/OrderService.Tests/TestData/test-data.json";

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"\n\nFile Not Found: {filePath}\n\n");
            throw new FileNotFoundException($"Test data file not found at: {filePath}");
        }

        var json = File.ReadAllText(filePath);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new DateTimeJsonConverter() }
        };

        var orderSeedDtos = JsonSerializer.Deserialize<TestData>(json, options)!.Orders;

        var orders = orderSeedDtos.Select(o => o.ToDomain()).ToList();

        return orders;
    }


    // Custom DateTime converter for JSON
    public class DateTimeJsonConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
        }
    }
}