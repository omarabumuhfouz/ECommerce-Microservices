/// <summary>
/// Thrown when a refresh token operation fails.
/// </summary>
public class RefreshTokenOperationException : Exception
{
    public RefreshTokenOperationException()
        : base() { }

    public RefreshTokenOperationException(string message) : base(message) { }

    public RefreshTokenOperationException(string message, Exception inner) : base(message, inner) { }
}
