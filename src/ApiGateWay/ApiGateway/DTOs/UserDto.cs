using System.Runtime.CompilerServices;
using Contracts.User;

namespace ApiGateway.DTOs;

public record UserDto(Guid Id, string Email, DateTime CreatedAt, List<string> Roles)
{
    public static UserDto FromUserModel(GetUserByIdResponse  response)
    {
        return new UserDto(
                        Guid.Parse(response.UserId),
                               response.Email,
                               response.CreatedAt.ToDateTime(),
                               response.Roles.ToList());
    }

}