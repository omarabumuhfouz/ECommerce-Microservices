using ApiGateway.Interfaces;
using Contracts.Order;

namespace ApiGateway.Services;

public class GrpcOrderClient : IOrderService
{
    private readonly OrderProtoService.OrderProtoServiceClient _grpcClient;

    public GrpcOrderClient(OrderProtoService.OrderProtoServiceClient grpcClient)
    {
        _grpcClient = grpcClient;
    }

    public async Task<int> GetOrdersCountsByCustomerIdAsync(Guid customerId)
    {
        return (await _grpcClient.GetOrdersCountByCustomerIdAsync(
                new GetOrdersCountByCustomerIdRequest { CustomerId = customerId.ToString() }
        ))
        .Counts;
    }
}