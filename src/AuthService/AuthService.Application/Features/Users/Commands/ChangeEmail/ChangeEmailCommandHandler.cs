using AuthService.Application.Features.Users.Specifications;

namespace AuthService.Application.Features.Users.Commands.ChangeEmail;

public class ChangeEmailCommandHandler : ICommandHandler<ChangeEmailCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ChangeEmailCommandHandler> _logger;

    public ChangeEmailCommandHandler(IUnitOfWork unitOfWork, ILogger<ChangeEmailCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(ChangeEmailCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting email change process for User ID: {UserId}", command.UserId);

        var userRepo = _unitOfWork.GetRepository<User>();

        var user = await userRepo.GetSingleBySpecAsync(new GetUserByIdSpec(command.UserId), cancellationToken);

        if (user is null)
        {
            _logger.LogWarning("User with ID: {UserId} not found.", command.UserId);
            return DomainErrors.User.NotFound(command.UserId);
        }

        if (user.Email.Equals(command.NewEmail, StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogInformation("Email for user {UserId} is already the same. No action needed.", command.UserId);
            return Unit.Value;
        }

        int count = await userRepo.CountAsync(new GetUserByEmailSpec(command.NewEmail), cancellationToken);
        if (count > 0)
        {
            _logger.LogWarning("Email {Email} is already in use.", command.NewEmail);
            return DomainErrors.User.EmailAlreadyExists;
        }

        var changeEmailResult = user.ChangeEmail(command.NewEmail);

        if (changeEmailResult.IsFailure)
        {
            _logger.LogError("Failed to change email for user {UserId}. Reason: {Error}", command.UserId, changeEmailResult.TopError);
            return changeEmailResult.TopError;
        }

        userRepo.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Email for user {UserId} changed successfully.", command.UserId);

        return Unit.Value;
    }
}
