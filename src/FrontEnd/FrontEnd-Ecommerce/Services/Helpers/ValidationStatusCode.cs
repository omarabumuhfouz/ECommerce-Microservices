using FrontEnd_Ecommerce.Bases;
using FrontEnd_Ecommerce.Exceptions;

public static class ValidationStatusCode
{
/// <summary>
    /// Validates the API response and ensures it was successful.
    /// </summary>
    /// <typeparam name="T">The type of the data contained in the API response.</typeparam>
    /// <param name="response">The API response object to validate.</param>
    /// <exception cref="NotFoundException">Thrown when the response status code is 404 (Not Found).</exception>
    /// <exception cref="UnauthorizedException">Thrown when the response status code is 401 (Unauthorized).</exception>
    /// <exception cref="BadRequestException">Thrown when the response status code is 400 (Bad Request).</exception>
    /// <exception cref="ConflictException">Thrown when the response status code is 409 (Conflict).</exception>
    /// <exception cref="InternalServerErrorException">Thrown when the response status code is 500 (Internal Server Error)
    /// or for any other unhandled error status.</exception>
    public static void EnsureRequestSuccess<T>(APIResponse<T> response)
    {
        if (response.Success ||  ((int)response.StatusCode >= 200 && (int)response.StatusCode <300))
            return;

        // Use switch statement instead of expression
        switch (response.StatusCode)
        {
            case HttpStatusCode.NotFound:
                throw new NotFoundException(response.Message ?? string.Empty);
            case HttpStatusCode.Unauthorized:
                throw new UnauthorizedException(response.Message ?? string.Empty);
            case HttpStatusCode.BadRequest:
                throw new BadRequestException(response.Message ?? string.Empty);
            case HttpStatusCode.Conflict:
                throw new ConflictException(response.Message ?? string.Empty);
            case HttpStatusCode.InternalServerError:
                throw new InternalServerErrorException(response.Message ?? string.Empty);
            default:
                throw new InternalServerErrorException(response.Message ?? string.Empty);
        }
    }
     
}