using Contracts.Order;
using Grpc.Core;
using PaymentService.Application.DTOs;
using PaymentService.Application.Services;
using PaymentService.Domain.Errors;
using SharedKernel.Shared;

namespace PaymentService.Infrastructure.Services;

public class GrpcOrderService : IOrderService
{
    private readonly OrderProtoService.OrderProtoServiceClient _orderClient;

    public GrpcOrderService(OrderProtoService.OrderProtoServiceClient orderClient)
        => _orderClient = orderClient;

    public async Task<Result<OrderDto>> GetOrderByIdAsync(Guid id)
    {
        try
        {
            var orderModel = await _orderClient.GetOrderByIdAsync(new GetOrderByIdRequest { OrderId = id.ToString() });

            return OrderDto.FromGrpcModel(orderModel);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            // Handle 404 specifically
            return DomainErrors.Order.NotFound(id);
        }
        catch (RpcException ex)
        {
            return DomainErrors.Order.GrpcError(ex.StatusCode.ToString(), ex.Status.Detail);
        }
        catch (Exception ex)
        {
            // Fallback for unexpected system failures
            return Result.Failure<OrderDto>( Error.Failure(
                "OrderService.UnexpectedError",
                ex.Message));
        }
    }

}