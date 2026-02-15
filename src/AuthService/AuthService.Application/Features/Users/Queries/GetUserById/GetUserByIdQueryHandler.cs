
using AuthService.Application.Features.Users.Queries.GetUserById;
using AuthService.Application.Features.Users.Specifications;

namespace AuthService.Queries;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    async Task<Result<UserDto>> IRequestHandler<GetUserByIdQuery, Result<UserDto>>.Handle(GetUserByIdQuery request, CancellationToken ct)
    {
        var user = await _unitOfWork.GetRepository<User>()
                    .GetSingleBySpecAsync(new GetUserByIdSpec(request.UserId), ct);

        if (user is null) return DomainErrors.User.NotFound(request.UserId);

        return UserDto.FromUser(user);
    }
}