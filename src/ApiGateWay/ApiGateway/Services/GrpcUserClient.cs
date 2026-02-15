using ApiGateway.DTOs;
using ApiGateway.Interfaces;
using Contracts.User;
using Grpc.Core;

namespace ApiGateway.Services;

public class GrpcUserClient : IUserService
{
    private readonly UserProtoService.UserProtoServiceClient _grpcClient;

    public GrpcUserClient(UserProtoService.UserProtoServiceClient grpcClient)
    {
        _grpcClient = grpcClient;
    }

    public async Task<UserDto> GetUserByIdOrThrowAsync(Guid userId)
    {
        try
        {
            return UserDto.FromUserModel(
                        await _grpcClient.GetUserByIdAsync(
                            new GetUserByIdRequest { UserId = userId.ToString() }));
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            throw new InvalidOperationException(ex.Message);
        }
        catch (RpcException ex)
        {
            throw new InvalidOperationException(ex.Message);
        }

    }

        
}