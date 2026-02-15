using ApiGateway.DTOs;

namespace ApiGateway.Interfaces;

public interface IUserService
{
    Task<UserDto> GetUserByIdOrThrowAsync(Guid userId);
}