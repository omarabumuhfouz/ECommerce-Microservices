using Contracts.Order;
using Grpc.Core;
using MediatR;
using OrderService.Application.Orders.Queries.GetOrderById;
using OrderService.Application.Orders.Queries.GetOrdersCountByCustomer;
using OrderService.Application.Orders.Queries.IsProductEligibleForFeedback;
using SharedKernel.Extensions;

namespace OrderService.Api.Services;


public class GrpcOrderServer(ISender sender) : OrderProtoService.OrderProtoServiceBase
{
    public override async Task<GetOrdersCountByCustomerIdResponse> GetOrdersCountByCustomerId(GetOrdersCountByCustomerIdRequest request, ServerCallContext context)
    {
        var result = await sender.Send(new GetOrdersCountsByCustomerIdQuery(Guid.Parse(request.CustomerId)));

        return new GetOrdersCountByCustomerIdResponse
        {
            Counts = result.Value
        };
    }

    public override async Task<OrderModel> GetOrderById(GetOrderByIdRequest request, ServerCallContext context)
    {
        var result = await sender.Send(new GetOrderByIdQuery(Guid.Parse(request.OrderId)));

        return result
                 .ValueOrThrowRpc()
                 .ToGrpcModel();
    }

    public override async Task<IsProductEligibleForFeedbackResponse> IsProductEligibleForFeedback(IsProductEligibleForFeedbackRequest request, ServerCallContext context)
    {
        var query = new IsProductEligibleForFeedbackQuery(Guid.Parse(request.CustomerId), Guid.Parse(request.ProductId));
        var result = await sender.Send(query);

        return new IsProductEligibleForFeedbackResponse
        {
            IsEligible = result.ValueOrThrowRpc()
        };
    }
}