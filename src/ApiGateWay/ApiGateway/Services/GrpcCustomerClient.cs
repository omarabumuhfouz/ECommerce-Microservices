using ApiGateway.DTOs;
using ApiGateway.Interfaces;
using Contracts.Customer;
using Grpc.Core;

namespace ApiGateway.Services;

public class GrpcCustomerClient : ICustomerService
{
    private readonly CustomerProtoService.CustomerProtoServiceClient _grpcClient;

    public GrpcCustomerClient(CustomerProtoService.CustomerProtoServiceClient grpcClient)
    {
        _grpcClient = grpcClient;
    }

    public async Task<CustomerDto> GetCustomerByUserIdOrThrowAsync(Guid userId)
    {
        try
        {
            return CustomerDto.FromCustomerModel(
                    await _grpcClient.GetByUserIDAsync(
                            new GetCustomerByUserIdRequest { UserId = userId.ToString() }
                    )
            );
        }
        catch(RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            throw new InvalidOperationException(ex.Message);
        }
        catch(RpcException ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }
}