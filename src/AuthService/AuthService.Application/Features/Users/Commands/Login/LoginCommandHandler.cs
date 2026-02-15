

using AuthService.Application.Features.Clients.Specifications;
using AuthService.Application.Features.RefreshTokens.DTOs;
using AuthService.Application.Features.Users.Specifications;
using AuthService.Application.Interfaces;

namespace AuthService.Application.Features.Users.Commands.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, TokenResponseDto>
{
    private readonly IPasswordService _passwordService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        IPasswordService passwordService,
        IJwtTokenService jwtTokenService,
        IUnitOfWork unitOfWork,
        ILogger<LoginCommandHandler> logger)
    {
        _passwordService = passwordService;
        _jwtTokenService = jwtTokenService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async Task<Result<TokenResponseDto>> IRequestHandler<LoginCommand, Result<TokenResponseDto>>.Handle(LoginCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Processing login attempt for Email: {Email}", request.Email);

        var clientRepo = _unitOfWork.GetRepository<Client>();
        var client = await clientRepo.GetSingleBySpecAsync(new GetClientByPublicIdSpec(request.ClientId), ct);

        if (client is null) return DomainErrors.Client.NotFound;

        var userRepo = _unitOfWork.GetRepository<User>();
        var user = await userRepo.GetSingleBySpecAsync(new GetUserByEmailSpec(request.Email), ct);

        if (user is null) return DomainErrors.User.InvalidCredentials;

        
        if (!_passwordService.ValidatePassword(request.Password, user.PasswordHash))
        {
            _logger.LogWarning("Login failed: Invalid password for UserId {UserId}.", user.Id);
            return DomainErrors.User.InvalidCredentials;
        }

        //  Generate Tokens
        var accessToken = _jwtTokenService.GenerateJwtToken(user, client);
        var rawRefreshToken = _jwtTokenService.GenerateRefreshToken();
        var hashedRefreshToken = _jwtTokenService.HashToken(rawRefreshToken);

        var refreshTokenResult = RefreshToken.Create(hashedRefreshToken, user.Id, client.Id);

        if (refreshTokenResult.IsFailure) return refreshTokenResult.TopError;

        await _unitOfWork.GetRepository<RefreshToken>().AddAsync(refreshTokenResult.Value, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        _logger.LogInformation("Login successful for UserId {UserId} on ClientId {ClientId}.", user.Id, client.Id);

        return new TokenResponseDto(accessToken, rawRefreshToken);
    }
}

