using AuthService.Application.Features.Users.Specifications;

namespace AuthService.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteUserCommandHandler> _logger;

    public DeleteUserCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteUserCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(DeleteUserCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Starting deletion process for User ID: {UserId}", request.UserId);

        var userRepo = _unitOfWork.GetRepository<User>();
        var user = await userRepo.GetSingleBySpecAsync(new GetUserByIdSpec(request.UserId), ct);

        if (user is null) return DomainErrors.User.NotFound(request.UserId);

        userRepo.Delete(user);
        await _unitOfWork.SaveChangesAsync(ct);

        _logger.LogInformation("User {UserId} deleted successfully.", request.UserId);

        return Unit.Value;
    }
}