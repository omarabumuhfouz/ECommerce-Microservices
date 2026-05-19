using AuthService.Application.Features.Users.Specifications;

namespace AuthService.Application.Features.Users.Commands.ChangeRole;

public class ChangeRoleCommandHandler : ICommandHandler<ChangeRoleCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ChangeRoleCommandHandler> _logger;

    public ChangeRoleCommandHandler(IUnitOfWork unitOfWork, ILogger<ChangeRoleCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(ChangeRoleCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Attempting to change roles for user {UserId}", request.UserId);

        var userRepo = _unitOfWork.GetRepository<User>();
        var user = await userRepo.FirstOrDefaultAsync(new GetUserByIdSpec(request.UserId), ct);
        if (user is null)
        {
            _logger.LogWarning("User with Id {UserId} not found", request.UserId);
            return DomainErrors.User.NotFound(request.UserId);
        }

        var roles = new List<UserRole>();
        foreach (var roleString in request.Roles)
        {
            if (Enum.TryParse<UserRole>(roleString, true, out var role))
            {
                roles.Add(role);
            }
            else
            {
                _logger.LogWarning("Invalid role string: {RoleString}", roleString);
                return DomainErrors.User.RoleInvalid;
            }
        }

        var result = user.ChangeRoles(roles);
        if (result.IsFailure)
        {
            _logger.LogWarning("Failed to change roles for user {UserId}. Error: {@Error}", request.UserId, result.TopError);
            return result.TopError;
        }

         userRepo.Update(user);
        await _unitOfWork.SaveChangesAsync(ct);

        _logger.LogInformation("Successfully changed roles for user {UserId}", request.UserId);

        return Unit.Value;
    }
}
