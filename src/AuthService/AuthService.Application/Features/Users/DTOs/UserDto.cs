using AuthService.Domain.Helpers;
using Contracts.User;
using Google.Protobuf.WellKnownTypes;

namespace AuthService.Application.Features.Users.DTOs;

public record UserDto(Guid UserId, string Email, DateTime CreatedAt ,List<string> Roles)
{
    public static UserDto FromUser(User user)
    {
        return new UserDto(user.Id, user.Email, user.CreatedAt, UserRoleHelper.GetRolesAsStrings(user.Roles));
    }

    public  GetUserByIdResponse ToGrpcModel()
    {
        var grpcModel = new GetUserByIdResponse
        {
            UserId = this.UserId.ToString(),
            Email = this.Email,
            CreatedAt = Timestamp.FromDateTime(this.CreatedAt),
        };

        grpcModel.Roles.AddRange(this.Roles);

        return grpcModel;
    }
}