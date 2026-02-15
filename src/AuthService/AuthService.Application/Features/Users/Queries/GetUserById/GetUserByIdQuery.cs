namespace AuthService.Application.Features.Users.Queries.GetUserById;

public record GetUserByIdQuery(Guid UserId) : IQuery<UserDto>;