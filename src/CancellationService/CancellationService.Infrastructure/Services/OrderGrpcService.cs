using CancellationService.Application.Cancellations.DTOs;
using CancellationService.Domain.Errors;
using Contracts.Order;
using Grpc.Core;
using SharedKernel.Shared;

namespace CancellationService.Infrastructure.Services;
public  class OrderGrpcService : IOrderService
{
    private readonly OrderProtoService.OrderProtoServiceClient _orderClient;

    public OrderGrpcService(OrderProtoService.OrderProtoServiceClient orderClient)
    {
        _orderClient = orderClient;
    }

    public async Task<Result<OrderDto>> GetOrderByIdAsync(Guid orderId)
    {
        try
        {
            var result = await _orderClient.GetOrderByIdAsync(new GetOrderByIdRequest { OrderId = orderId.ToString() });

        return OrderDto.FromModel(result);


        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            return DomainErrors.Order.NotFound(orderId);

        }
        catch (RpcException ex)
        {
            // Log the exception as needed
            return Error.Unexpected("An unexpected error occurred while fetching the order details.");
        }
    }

}