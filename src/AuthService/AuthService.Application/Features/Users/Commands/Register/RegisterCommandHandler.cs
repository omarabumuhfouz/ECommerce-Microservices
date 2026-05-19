using AuthService.Application.Features.Clients.Specifications;
using AuthService.Application.Features.RefreshTokens.DTOs;
using AuthService.Application.Features.Users.Specifications;
using AuthService.Application.Interfaces;
using AuthService.Domain.Helpers;
using MassTransit;
using SharedKernel.IntegrationEvents.Users;

namespace AuthService.Features.Users.Commands.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, RegisterResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordService _passwordService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordService passwordService,
        IJwtTokenService jwtTokenService,
        IPublishEndpoint publishEndpoint,
        ILogger<RegisterCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _passwordService = passwordService;
        _jwtTokenService = jwtTokenService;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    async Task<Result<RegisterResponseDto>> IRequestHandler<RegisterCommand, Result<RegisterResponseDto>>.Handle(RegisterCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Starting registration process for Email: {Email} on ClientId: {ClientId}", request.Email, request.ClientId);

        // 1. Validation & Pre-computation (Client check, Email uniqueness, Password Hash)
        var contextResult = await GetRegisterContextAsync(request.Email, request.ClientId, request.Password, ct);
        
        if (contextResult.IsFailure)
        {
            _logger.LogWarning("Registration failed during validation. Error: {@Error}", contextResult.TopError);
            return contextResult.TopError;
        }

        // Deconstruct the Result Value safely
        var (client, newUser) = contextResult.Value;

        await _publishEndpoint.Publish(new UserRegisteredEvent(
        newUser.Id,
        request.FirstName,
        request.LastName,
        request.Email,
        request.PhoneNumber
        ));

        // 3. Prepare Local Database Entities
        await _unitOfWork.GetRepository<User>().AddAsync(newUser, ct);

        // 4. Generate Tokens
        var accessToken = _jwtTokenService.GenerateJwtToken(newUser, client);
        var refreshTokenRaw = _jwtTokenService.GenerateRefreshToken();
        var hashedRefreshToken = _jwtTokenService.HashToken(refreshTokenRaw);

        var refreshTokenResult = RefreshToken.Create(hashedRefreshToken, newUser.Id, client.Id);
        
        if (refreshTokenResult.IsFailure)
        {
            // If this fails, we must ideally revert the Customer Service call (Compensating Transaction)
            return refreshTokenResult.TopError;
        }

        await _unitOfWork.GetRepository<RefreshToken>().AddAsync(refreshTokenResult.Value, ct);

        // 5. Commit Transaction
        await _unitOfWork.SaveChangesAsync(ct);

        _logger.LogInformation("Registration completed successfully. UserId: {UserId}, Email: {Email}.", newUser.Id, newUser.Email);

        // 6. Map Response
        var profile = new ProfileDto(
            newUser.Id,
            request.FirstName,
            request.LastName,
            request.Email,
            request.PhoneNumber,
            UserRoleHelper.GetRolesAsStrings(newUser.Roles)
        );

        var tokenInfo = new TokenResponseDto(accessToken, refreshTokenRaw);

        return new RegisterResponseDto(profile, tokenInfo);
    }

    /// <summary>
    /// Helper to validate uniqueness and prepare the User entity.
    /// Returns a Tuple of (Client, User) wrapped in a Result.
    /// </summary>
    private async Task<Result<(Client, User)>> GetRegisterContextAsync(string email, string clientId, string password, CancellationToken ct)
    {
        // Check 1: Client Application Exists
        // Uses the Public ID Specification (String -> Guid resolution)
        var client = await _unitOfWork.GetRepository<Client>()
                                      .FirstOrDefaultAsync(new GetClientByPublicIdSpec(clientId), ct);

        if (client is null)
        {
             // Logged in the main handler via TopError
            return DomainErrors.Client.NotFound; 
        }

        // Check 2: User Email Uniqueness
        var isEmailExists = await _unitOfWork.GetRepository<User>()
                                             .AnyAsync(new GetUserByEmailSpec(email), ct);

        if (isEmailExists)
        {
            return DomainErrors.User.EmailAlreadyExists;
        }

        // Create User Entity (In-Memory)
        var newUserResult = User.Create(
            email,
            _passwordService.HashPassword(password),
            new List<UserRole> { UserRole.Customer }
        );

        if (newUserResult.IsFailure) return newUserResult.TopError;

        return (client, newUserResult.Value);
    }
}
