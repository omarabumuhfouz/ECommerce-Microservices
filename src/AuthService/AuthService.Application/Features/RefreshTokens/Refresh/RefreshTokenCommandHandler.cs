using AuthService.Application.Features.Clients.Specifications;
using AuthService.Application.Features.RefreshTokens.DTOs;
using AuthService.Application.Features.RefreshTokens.Specifications;
using AuthService.Application.Features.Users.Specifications;
using AuthService.Application.Interfaces;

namespace AuthService.Application.Features.RefreshTokens.Refresh;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, TokenResponseDto>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RefreshTokenCommandHandler> _logger;

    public RefreshTokenCommandHandler(
        IJwtTokenService jwtTokenService,
        IUnitOfWork unitOfWork,
        ILogger<RefreshTokenCommandHandler> logger)
    {
        _jwtTokenService = jwtTokenService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async Task<Result<TokenResponseDto>> IRequestHandler<RefreshTokenCommand, Result<TokenResponseDto>>.Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        // 1. Resolve Client (Public String -> Guid)
        var client = await _unitOfWork.GetRepository<Client>()
            .GetSingleBySpecAsync(new GetClientByPublicIdSpec(request.ClientId), ct);

        if (client is null) return DomainErrors.Client.NotFound;

        // 2. Hash incoming token for lookup
        var hashedToken = _jwtTokenService.HashToken(request.RefreshToken);

        var refreshTokenRepo = _unitOfWork.GetRepository<RefreshToken>();
        
        // 3. Find the exact token
        var storedRefreshToken = await refreshTokenRepo.GetSingleBySpecAsync(
            new GetRefreshTokenByTokenAndClientSpec(hashedToken, client.Id), ct);

        if (storedRefreshToken is null) 
        {
            // Security Note: If we can't find the token, it might be a forgery.
            _logger.LogWarning("Security Alert: Attempt to refresh with non-existent token for Client {ClientId}.", request.ClientId);
            return DomainErrors.RefreshToken.Invalid;
        }

        // 4. Reuse Detection (Critical Security Feature)
        // If a token is ALREADY revoked, it means someone (hacker) is trying to use an old token 
        // that the legitimate user already rotated. 
        if (storedRefreshToken.IsRevoked)
        {
            _logger.LogError("Security Alert: Token Reuse Detected for User {UserId}!", storedRefreshToken.UserId);

            // 1. Find all other active tokens for this user
            var allActiveSpec = new GetActiveRefreshTokensByUserIdSpec(storedRefreshToken.UserId);

            var allTokens = await _unitOfWork.GetRepository<RefreshToken>()
                                             .ListAsync(allActiveSpec, ct);

            // 2. Revoke them all immediately
            foreach (var token in allTokens)
            {
                token.Revoke();
            }

            // 3. Save Changes immediately to lock the account out
            await _unitOfWork.SaveChangesAsync(ct);

            // 4. Return the error
            return DomainErrors.RefreshToken.Revoked;
        }

        // 5. Expiration Check
        // Tip: Ensure IsActive property in Entity handles this logic centrally if possible
        if (storedRefreshToken.ExpiresAt < DateTime.UtcNow) 
        {
            return DomainErrors.RefreshToken.Expired;
        }

        // 6. Retrieve User (Only now do we pay the cost to fetch the user)
        var user = await _unitOfWork.GetRepository<User>()
            .GetSingleBySpecAsync(new GetUserByIdSpec(storedRefreshToken.UserId), ct);

        if (user is null) return DomainErrors.User.NotFound(storedRefreshToken.UserId);


        // A. Revoke the OLD token (Prevent Reuse)
        storedRefreshToken.Revoke();

        // B. Create the NEW token
        var newRefreshTokenRaw = _jwtTokenService.GenerateRefreshToken();
        var hashedNewToken = _jwtTokenService.HashToken(newRefreshTokenRaw);

        var newRefreshTokenResult = RefreshToken.Create(hashedNewToken, user.Id, client.Id);
        
        if (newRefreshTokenResult.IsFailure) return newRefreshTokenResult.TopError;

        await refreshTokenRepo.AddAsync(newRefreshTokenResult.Value, ct);

        // C. Generate new JWT
        var newJwtToken = _jwtTokenService.GenerateJwtToken(user, client);

        // D. Commit everything (Revoke Old + Add New)
        await _unitOfWork.SaveChangesAsync(ct);

        _logger.LogInformation("Token Rotation Successful: User {UserId}, Client {ClientId}.", user.Id, client.Id);

        return new TokenResponseDto(newJwtToken, newRefreshTokenRaw);
    }
}