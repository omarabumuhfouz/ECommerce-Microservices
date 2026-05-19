using AuthService.Application.Features.Clients.Specifications;
using AuthService.Application.Features.RefreshTokens.Specifications;
using AuthService.Application.Interfaces;

namespace AuthService.Features.Users.Commands.Logout;

public class LogoutCommandHandler : ICommandHandler<LogoutCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<LogoutCommandHandler> _logger;

    public LogoutCommandHandler(
        IUnitOfWork unitOfWork,
        IJwtTokenService jwtTokenService,
        ILogger<LogoutCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenService = jwtTokenService;
        _logger = logger;
    }

    async Task<Result<Unit>> IRequestHandler<LogoutCommand, Result<Unit>>.Handle(LogoutCommand request, CancellationToken ct)
    {
        // 1. Resolve Client Guid from the Public Client ID string
        var clientSpec = new GetClientByPublicIdSpec(request.ClientId);
        var client = await _unitOfWork.GetRepository<Client>()
                                      .FirstOrDefaultAsync(clientSpec, ct);

        if (client is null) return DomainErrors.Client.NotFound;

        // 2. Locate the specific token (Hashing required for lookup)
        var hashedToken = _jwtTokenService.HashToken(request.RefreshToken);
    var tokenSpec = new GetRefreshTokenForRotationSpec(hashedToken, request.UserId, client.Id, true);

        var tokenRepo = _unitOfWork.GetRepository<RefreshToken>();
        var storedRefreshToken = await tokenRepo.FirstOrDefaultAsync(tokenSpec, ct);


        if (storedRefreshToken is null) return DomainErrors.RefreshToken.Invalid;

        // Idempotency: If already revoked, consider the logout successful to avoid client errors.
        if (storedRefreshToken.IsRevoked) return Unit.Value;

        // 3. Perform Revocation
        storedRefreshToken.Revoke();

        if (request.IsLogoutFromAllDevices)
        {
            var allActiveSpec = new GetActiveRefreshTokensByUserIdSpec(storedRefreshToken.UserId);
            var userRefreshTokens = await tokenRepo.GetListAsync(allActiveSpec, ct);

            foreach (var token in userRefreshTokens)
            {
                token.Revoke();
            }

            _logger.LogInformation("Security Audit: User {UserId} initiated 'Logout All Devices'.", request.UserId);
        }
        else
        {
            _logger.LogInformation("Security Audit: User {UserId} logged out from Client {ClientId}.", request.UserId, request.ClientId);
        }
        
        await _unitOfWork.SaveChangesAsync(ct);

        return Unit.Value;
    }
}