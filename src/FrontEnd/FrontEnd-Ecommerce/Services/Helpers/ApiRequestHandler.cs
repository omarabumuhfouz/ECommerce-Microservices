using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using FrontEnd_Ecommerce.Exceptions;

namespace FrontEnd_Ecommerce.Services.Helpers;

public class ApiRequestHandler
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiRequestHandler> _logger;
    private readonly ICookieManager _cookieManager;
    private readonly IRequestRetryHandler _retryHandler;

    private static readonly JsonSerializerOptions _jsonOptions = new() 
    { 
        PropertyNameCaseInsensitive = true 
    };

    public ApiRequestHandler(
        IHttpClientFactory httpClientFactory,
        ILogger<ApiRequestHandler> logger,
        ICookieManager cookieManager,
        IRequestRetryHandler retryHandler) 
    {
        _httpClient = httpClientFactory.CreateClient("ApiGateway");
        _logger = logger;
        _cookieManager = cookieManager;
        _retryHandler = retryHandler;
    }

    public async Task SendRequestAsync(ApiRequestSettings settings)
    {
        await _retryHandler.ExecuteWithRetryAsync(async () => 
        {
            await ExecuteInternalAsync(settings);
            return true;
        });
    }

    private async Task ExecuteInternalAsync(ApiRequestSettings settings)
    {
        try 
        {
            _logger.LogInformation("Executing {Method} request to {Url}", settings.Method, settings.Url);

            using var request = BuildRequest(settings);
            using var response = await _httpClient.SendAsync(request);

            // 204 No Content is Success
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                _logger.LogInformation("{Url} completed with 204 No Content", settings.Url);
                return;
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                
                // 📝 LOG: API Failure (4xx/5xx)
                _logger.LogWarning("API Error {StatusCode} for {Url}. Response: {Content}", 
                    response.StatusCode, settings.Url, errorContent);

                throw new ApiException((int)response.StatusCode, errorContent, $"Request failed: {response.StatusCode}");
            }

            // 📝 LOG: Success
            _logger.LogInformation("Request to {Url} succeeded ({StatusCode})", settings.Url, response.StatusCode);
        }
        catch (Exception ex) when (ex is not ApiException) 
        {
            _logger.LogError(ex, "Unexpected network error calling {Url}", settings.Url);
            throw; 
        }
    }

    public async Task<T?> SendRequestAsync<T>(ApiRequestSettings settings)
    {
        return await _retryHandler.ExecuteWithRetryAsync(async () => 
        {
            return await ExecuteInternalAsync<T>(settings);
        });
    }

    private async Task<T?> ExecuteInternalAsync<T>(ApiRequestSettings settings)
    {
        try
        {
            _logger.LogInformation("Executing {Method} request to {Url} [Expects: {Type}]", 
                settings.Method, settings.Url, typeof(T).Name);

            using var request = BuildRequest(settings);
            using var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.NoContent) 
            {
                _logger.LogInformation("{Url} returned 204 No Content (Returning Default)", settings.Url);
                return default;
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();

                _logger.LogWarning("API Error {StatusCode} for {Url}. Response: {Content}", 
                    response.StatusCode, settings.Url, errorContent);

                throw new ApiException((int)response.StatusCode, errorContent, $"Request failed: {response.StatusCode}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(jsonResponse)) 
            {
                _logger.LogWarning("{Url} returned 200 OK but Body was Empty", settings.Url);
                return default;
            }

            _logger.LogInformation("Received {Bytes} chars from {Url}", jsonResponse.Length, settings.Url);

            return JsonSerializer.Deserialize<T>(jsonResponse, _jsonOptions);
        }
        catch (Exception ex) when (ex is not ApiException)
        {
            _logger.LogError(ex, "Unexpected network error calling {Url}", settings.Url);
            throw;
        }
    }

    private HttpRequestMessage BuildRequest(ApiRequestSettings settings)
    {
        var request = new HttpRequestMessage(settings.Method, settings.Url);
        var token = _cookieManager.GetAccessToken();

        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        if (settings.Body is not null)
        {
            var jsonData = JsonSerializer.Serialize(settings.Body, _jsonOptions);
            request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            
            _logger.LogDebug("Payload for {Url}: {Json}", settings.Url, jsonData);
        }

        foreach (var header in settings.Headers.GetAll())
            request.Headers.Add(header.Key, header.Value);

        return request;
    }
}