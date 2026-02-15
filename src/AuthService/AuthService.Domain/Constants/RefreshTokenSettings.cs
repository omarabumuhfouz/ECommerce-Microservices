namespace AuthService.Domain.Constants;

/// <summary>
/// Holds constants for refresh token settings.
/// </summary>
public static class RefreshTokenSettings
{
    /// <summary>
    /// The number of days a refresh token is valid before expiration.
    /// </summary>
    public const int ExpiresInDays = 7;
}
