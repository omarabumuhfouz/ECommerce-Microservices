using FrontEnd_Ecommerce.Enums;

public class ApiRequestSettings
{
    public HttpMethod Method { get; set; } = HttpMethod.Get;
    public string Url { get; set; } = "";
    public object? Body { get; set; } = null;
    public HeaderBuilder Headers { get; set; } = new();


    public ApiRequestSettings WithBody(object body)
    {
        Body = body;
        return this;
    }

    public ApiRequestSettings UsingMethod(HttpMethod method)
    {
        Method = method;
        return this;
    }

    public ApiRequestSettings ToUrl(string url)
    {
        Url = url;
        return this;
    }


    /// <summary>
    /// Allows configuring headers using a lambda.
    /// </summary>
    /// <param name="configure">Lambda to configure headers via HeaderBuilder</param>
    /// <returns>The current ApiRequestSettings instance</returns>
    public ApiRequestSettings WithHeaders(Action<HeaderBuilder> configure)
    {
        configure?.Invoke(Headers);
        return this;
    }
    
}
