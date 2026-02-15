using ShoppingCartService.Application.DTOs;
using System.Text.Json;
using System.Text.Json.Serialization;

public class TestDataLoader
{
    public class TestData
    {
        public List<CartTestDto> Carts { get; set; }
    }

    public static List<Cart> LoadData(IMapper mapper)
    {

        var filePath = "/home/omar/Downloads/MicroservicesProjects/ECommerce-Microservices/src/ShoppingCartService/ShoppingCartService.Tests/TestData/test-data.json";

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

        var cartSeedDtos = JsonSerializer.Deserialize<TestData>(json, options)!.Carts;


        var carts = cartSeedDtos.Select(cdto => new Cart(
            id: cdto.Id,
            customerId: cdto.CustomerId,
            isCheckedOut: cdto.IsCheckedOut,
            createdAt: cdto.CreatedAt,
            updatedAt: cdto.UpdatedAt,
            cartItems: cdto.CartItems.Select(idto => new CartItem(
                id: idto.Id,
                cartId: cdto.Id,
                productId: idto.ProductId,
                quantity: idto.Quantity,
                unitPrice: idto.UnitPrice,
                discount: idto.Discount,
                createdAt: idto.CreatedAt,
                updatedAt: idto.UpdatedAt
            )).ToList()

        )).ToList();

        return carts;
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