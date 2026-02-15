using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace FrontEnd_Ecommerce.Services.Helpers;

// My Note : Using Caching here 
public class JsonStringLocalizer : IStringLocalizer
{
    private readonly IDistributedCache _cache;
    private readonly JsonSerializer _serializer = new();

    public JsonStringLocalizer(IDistributedCache cache)
    {
        _cache = cache;
    }

    public LocalizedString this[string name] => new LocalizedString(name, GetString(name));

    public LocalizedString this[string name, params object[] arguments]
        => !this[name].ResourceNotFound
           ? new LocalizedString(name, string.Format(this[name].Value, arguments))
           : this[name];

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        var filePath = GetJsonFilePath();

        using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        using StreamReader stremReader = new(fileStream);
        using JsonTextReader jsonReader = new(stremReader);

        while (jsonReader.Read())
        {
            if (jsonReader.TokenType != JsonToken.PropertyName)
                continue;

            var key = jsonReader.Value as string;
            jsonReader.Read();
            var value = _serializer.Deserialize<string>(jsonReader) ?? string.Empty;
            yield return new LocalizedString(key, value, false);
        }
    }


    private string GetString(string key)
    {
        var fullFilePath = GetJsonFilePath();
        Console.WriteLine($"FullFilePath : {fullFilePath}");

        if (!File.Exists(fullFilePath))
        {
            Console.WriteLine("\n\nFile Not Exists\n\n");
            Console.WriteLine($"\n\n{fullFilePath}");
            //throw new FileNotFoundException($"Localization file not found: {filePath}"); // can return empty string
            return string.Empty;
        }

        // locale_en-US_welcome
        // locale_ar-JO_welcom
        var cacheKey = $"locale_{Thread.CurrentThread.CurrentCulture.Name}_{key}";
        var cacheValue = _cache.GetString(cacheKey);
        if (!string.IsNullOrEmpty(cacheValue))
            return cacheValue;

        var result = GetValueFromJson(key, fullFilePath);

        if (!string.IsNullOrEmpty(result))
            _cache.SetString(cacheKey, result);

        return result;
    }

    private string GetValueFromJson(string propertyName, string filePath)
    {
        if (string.IsNullOrEmpty(propertyName) || string.IsNullOrEmpty(filePath))
        {
            return string.Empty; // can return empty string
            //throw new ArgumentNullException(nameof(propertyName), "Property name cannot be null or empty."); // can return empty string
        }

        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
        {
            return string.Empty; // can return empty string
        }
        using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        using StreamReader stremReader = new(fileStream);
        using JsonTextReader jsonReader = new(stremReader);

        while (jsonReader.Read())
        {
            if (jsonReader.TokenType == JsonToken.PropertyName &&
                jsonReader.Value?.ToString() == propertyName)
            {
                jsonReader.Read(); // move to the value
                return _serializer.Deserialize<string>(jsonReader) ?? string.Empty;
            }
        }

        return string.Empty;
    }

    private string GetJsonFilePath()
    {
        return Path.Combine(AppContext.BaseDirectory, "Resources", $"{Thread.CurrentThread.CurrentCulture.Name}.json");

    }

}
