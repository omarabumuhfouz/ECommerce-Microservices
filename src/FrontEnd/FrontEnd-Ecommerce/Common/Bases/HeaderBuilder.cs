using System.Globalization;

public class HeaderBuilder
{
    private readonly Dictionary<string, string> _headers = new();

    public HeaderBuilder AddAuthorization(string token)
    {
        if (!string.IsNullOrEmpty(token))
            _headers["Authorization"] = $"Bearer {token}";

        return this;
    }

    public HeaderBuilder AddAcceptLanguage(string? culture = null)
    {
        _headers["Accept-Language"] = culture is null ? CultureInfo.CurrentCulture.Name : culture;

        return this;
    }

    public HeaderBuilder AddHeader(string name, string value)
    {
        _headers[name] = value;

        return this;
    }

    public IReadOnlyDictionary<string, string> GetAll() => _headers;
}
