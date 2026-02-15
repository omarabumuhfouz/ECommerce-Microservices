using System.Text.Json;
using System.Text.Json.Serialization;
using ProductService.Application.DTOs;


public class TestDataLoader
{
    public class TestData
    {
        public List<CategorySeedDto> Categories { get; set; }
    }

    public static List<Category> LoadData(IMapper mapper)
    {
        var filePath = "D:\\MicroservicesProjects\\ECommerce-Microservices\\src\\ProductService\\ProductService.Tests\\TestData\\test-data.json";

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

        var categorySeedDtos = JsonSerializer.Deserialize<TestData>(json, options)!.Categories;

        //         var categories = categorySeedDtos.Select(dto => new Category(
        //     dto.Id,
        //     dto.Name,
        //     dto.Description,
        //     dto.IsActive,
        //     dto.Products.Select(p => Product.Create(
        //         p.Name,
        //         p.Description,
        //         p.Price,
        //         p.CategoryId,
        //         p.StockQuantity,
        //         p.DiscountPercentage,
        //         p.ImageUrl
        //     )).ToList()
        // )).ToList();
        //         return mapper.Map<List<Category>>(categories);

        //         // Map and return pure Customer domain objects
        //         //return mapper.Map<List<Category>>(testData.Categories);
        //     }

        return null;

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