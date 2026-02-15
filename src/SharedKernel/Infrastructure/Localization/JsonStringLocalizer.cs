using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SharedKernel.Localization;

public class JsonStringLocalizer : IStringLocalizer
{
    private readonly IDistributedCache _cache;
    private readonly string _culture;

    public JsonStringLocalizer(IDistributedCache cache)
    {
        _cache = cache;
        _culture = Thread.CurrentThread.CurrentCulture.Name;
    }

    public LocalizedString this[string name] => new LocalizedString(name, GetString(name));

    public LocalizedString this[string name, params object[] arguments]
        => !this[name].ResourceNotFound
           ? new LocalizedString(name, string.Format(this[name].Value, arguments))
           : this[name];

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
{
        var jObject = GetJsonRoot();
        if (jObject == null)
        {
            return Enumerable.Empty<LocalizedString>();
        }

        // This will flatten the nested JObject into a list of key-value pairs
        return jObject.Descendants()
        .Where(p => p is JValue)
            .Select(p => new LocalizedString(p.Path.Replace(".", ":"), p.ToString(), false));
    }

    private string GetString(string key)
    {
        var jObject = GetJsonRoot();
        if (jObject == null)
        {
            return string.Empty; 
        }

        string jsonPath = key.Replace(":", ".");

        var token = jObject.SelectToken(jsonPath);

        return token?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Loads the entire JSON file for the current culture into a JObject
    /// and caches it.
    /// </summary>
    private JObject? GetJsonRoot()
    {
        string cacheKey = $"locale_{_culture}";
        var cachedJson = _cache.GetString(cacheKey);
        
        string jsonContent;

        if (string.IsNullOrEmpty(cachedJson))
        {
            var filePath = GetJsonFilePath();
            if (!File.Exists(filePath))
            {
                return null; 
            }

            jsonContent = File.ReadAllText(filePath);

            _cache.SetString(cacheKey, jsonContent);
        }
        else
        {
            jsonContent = cachedJson;
        }

        return JObject.Parse(jsonContent);
    }
    
    private string GetJsonFilePath()
    {
        string baseDirectory = AppContext.BaseDirectory;

        return Path.Combine(baseDirectory, "Localization", "Resources", $"{_culture}.json");
    }
}
