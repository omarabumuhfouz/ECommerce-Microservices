using AuthService.Application.Features.Users.Specifications;
using AuthService.Application.Interfaces;

namespace AuthService.Application.Features.Users.Commands.ChangePassword;

public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordService _passwordService;
    private readonly ILogger<ChangePasswordCommandHandler> _logger;

    public ChangePasswordCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordService passwordService,
        ILogger<ChangePasswordCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _passwordService = passwordService;
        _logger = logger;
    }

    async Task<Result<Unit>> IRequestHandler<ChangePasswordCommand, Result<Unit>>.Handle(ChangePasswordCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Starting Change Password process for UserId: {@UserId}", request.UserId);

        var userRepo = _unitOfWork.GetRepository<User>();
        var user = await userRepo.GetSingleBySpecAsync(new GetUserByIdSpec(request.UserId, true), ct);

        if (user is null)
        {
            _logger.LogWarning("Change Password Failed: User with Id {@UserId} was not found.", request.UserId);
            return DomainErrors.User.NotFound(request.UserId);
        }


        if (!_passwordService.ValidatePassword(request.CurrentPassword, user.PasswordHash)) 
        {
            _logger.LogWarning("Change Password Failed: Invalid current password provided for UserId {@UserId}.", request.UserId);
            return DomainErrors.User.InvalidCredentials;
        }

        var changeResult = user.ChangePassword(_passwordService.HashPassword(request.NewPassword));

        if (changeResult.IsFailure) 
        {
            _logger.LogWarning("Change Password Failed: Domain validation error for UserId {@UserId}. Error: {@Error}", 
                request.UserId, 
                changeResult.TopError);
                
            return changeResult.TopError;
        }

        await _unitOfWork.SaveChangesAsync(ct);

        _logger.LogInformation("Change Password Successfully for UserId : {@UserId}", user.Id);

        return Unit.Value;
    }
}
