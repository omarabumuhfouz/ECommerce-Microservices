using AuthService.Domain.Constants;

namespace AuthService.Domain.RefreshTokens; 

public record RefreshToken : AggregateRoot 
{
    private RefreshToken() { }

    private RefreshToken(Guid id, string token, Guid userId, Guid clientId, DateTime expiresAt) : base(id)
    {
        Token = token;
        UserId = userId;
        ClientId = clientId;
        ExpiresAt = expiresAt;
        CreatedAt = DateTime.UtcNow;
        IsRevoked = false;
    }

    public string Token { get; private set; } 
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }

    public Guid UserId { get; private set; }
    public Guid ClientId { get; private set; }

    public static Result<RefreshToken> Create(string token, Guid userId, Guid clientId)
    {
        if (string.IsNullOrWhiteSpace(token)) return DomainErrors.RefreshToken.TokenRequired;

        if (userId == Guid.Empty) return DomainErrors.RefreshToken.UserIdRequired;

        if (clientId == Guid.Empty) return DomainErrors.RefreshToken.ClientIdRequired;

        var refreshToken = new RefreshToken(
            Guid.NewGuid(),
            token,
            userId,
            clientId,
            DateTime.UtcNow.AddDays(RefreshTokenSettings.ExpiresInDays)
        );

        return Result.Success(refreshToken);
    }

    public void Revoke()
    {
        if (IsRevoked) return; 

        IsRevoked = true;
        RevokedAt = DateTime.UtcNow;
    }
    
    public bool IsActive => !IsRevoked && DateTime.UtcNow < ExpiresAt;
}
