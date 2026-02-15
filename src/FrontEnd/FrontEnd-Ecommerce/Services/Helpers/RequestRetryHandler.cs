using FrontEnd_Ecommerce.Exceptions;

namespace FrontEnd_Ecommerce.Services.Helpers;

public class RequestRetryHandler(ITokenService tokenService) : IRequestRetryHandler
{
    public async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> action)
    {
        try
        {
            return await action();
        }
        catch (ApiException ex) when (ex.StatusCode == 401)
        {
            // 2. Catch 401 -> Refresh Token
            var refreshSuccess = await tokenService.RefreshTokenAsync(); // Uses your new TokenService

            if (refreshSuccess) return await action();

            throw; // If refresh fails, bubble up the 401
        }
    }
}