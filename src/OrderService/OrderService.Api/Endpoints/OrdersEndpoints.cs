#region Usings

using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Api.Contracts.Orders;
using OrderService.Application.Orders.Commands.AddOrderItem;
using OrderService.Application.Orders.Commands.CreateOrder;
using OrderService.Application.Orders.Commands.RemoveOrderItem;
using OrderService.Application.Orders.Commands.UpdateOrderItem;
using OrderService.Application.Orders.Commands.UpdateOrderItems;
using OrderService.Application.Orders.Commands.UpdateOrderStatusToProcessing;
using OrderService.Application.Orders.Commands.UpdateOrderStatusToShipped;
using OrderService.Application.Orders.Commands.UpdateOrderStatusToDelivered;
using OrderService.Application.Orders.Commands.UpdateOrderStatusToCanceled;
using OrderService.Application.Orders.Commands.UpdateOrderStatusToRefunded;
using OrderService.Application.Orders.DTOs;
using OrderService.Application.Orders.Queries.GetOrderById;
using OrderService.Application.Orders.Queries.GetOrders;
using OrderService.Application.Orders.Queries.GetOrdersByCustomer;
using OrderService.Application.Orders.Queries.IsProductEligibleForFeedback;
using SharedKernel.Extensions;
using SharedKernel.Constants;

#endregion

namespace OrderService.Api.Endpoints;

public static class OrdersEndpoints
{
    public static RouteGroupBuilder MapOrdersEndpoints(this IEndpointRouteBuilder app)
    {
        var ordersApi = app.MapGroup("/api/orders")
                            .WithTags("Orders")
                            .WithOpenApi();

        // 🛒 Create Order (Shared - Customers place orders)
        ordersApi.MapPost("/", CreateOrder)
            // .RequireAuthorization(AuthConstants.Policies.Shared) 
            .Accepts<CreateOrderRequest>("application/json")
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a new order")
            .WithName("CreateOrder");

        // 🔍 View Specific Order (Shared - User views own, Admin views any)
        ordersApi.MapGet("/{orderId:Guid}", GetOrderById)
            // .RequireAuthorization(AuthConstants.Policies.Shared) 
            .Produces<OrderDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Get an order by ID")
            .WithName("GetOrderById");


        ordersApi.MapPut("/{orderId:Guid}/status/processing", UpdateOrderStatusToProcessing)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly)
            .Accepts<UpdateStatusRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Update order status to Processing")
            .WithName("UpdateOrderStatusToProcessing");

        ordersApi.MapPut("/{orderId:Guid}/status/shipped", UpdateOrderStatusToShipped)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Update order status to Shipped")
            .WithName("UpdateOrderStatusToShipped");

        ordersApi.MapPut("/{orderId:Guid}/status/delivered", UpdateOrderStatusToDelivered)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly)
             .Accepts<UpdateStatusRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Update order status to Delivered")
            .WithName("UpdateOrderStatusToDelivered");

        ordersApi.MapPut("/{orderId:Guid}/status/canceled", UpdateOrderStatusToCanceled)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Update order status to Canceled")
            .WithName("UpdateOrderStatusToCanceled");

        ordersApi.MapPut("/{orderId:Guid}/status/refunded", UpdateOrderStatusToRefunded)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Update order status to Refunded")
            .WithName("UpdateOrderStatusToRefunded");

        // 📋 View All Orders (Admin Only - Reporting/Management)
        ordersApi.MapGet("/", GetOrders)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly) 
            .Produces<List<OrderDto>>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Get all orders")
            .WithName("GetOrders");

        // 👤 View Customer History (Shared - User views own, Admin views user's)
        ordersApi.MapGet("/customer/{customerId:guid}", GetOrdersByCustomer)
            // .RequireAuthorization(AuthConstants.Policies.Shared) 
            .Produces<List<OrderDto>>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Get orders by customer")
            .WithName("GetOrdersByCustomer");

        // ✅ Feedback Check (Shared - For frontend logic)
        ordersApi.MapGet("/feedback-eligible", IsProductEligibleForFeedback)
            // .RequireAuthorization(AuthConstants.Policies.Shared) 
            .Produces<bool>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Check feedback eligibility")
            .WithName("IsProductEligibleForFeedback");

        
        // Add Order Item
        ordersApi.MapPost("/{orderId:Guid}/items", AddOrderItem)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly)
            .Accepts<CreateItemDto>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Add an item to an order")
            .WithName("AddOrderItem");

        // Update Order Item
        ordersApi.MapPut("/{orderId:Guid}/items/{itemId:Guid}", UpdateOrderItem)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly)
            .Accepts<UpdateItemRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Update an item in an order")
            .WithName("UpdateOrderItem");

        // Remove Order Item
        ordersApi.MapDelete("/{orderId:Guid}/items/{itemId:Guid}", RemoveOrderItem)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Remove an item from an order")
            .WithName("RemoveOrderItem");
        
        // Update Order Items
        ordersApi.MapPut("/{orderId:Guid}/items", UpdateOrderItems)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly)
            .Accepts<List<UpdateItemsDto>>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Update all items in an order")
            .WithName("UpdateOrderItems");

        return ordersApi;
    }

    private static async Task<IResult> CreateOrder(
        [FromBody] CreateOrderRequest request,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new CreateOrderCommand(
            request.CustomerId,
            request.BillingAddressId,
            request.ShippingAddressId,
            request.OrderItems
        ));

        return result.Match(
            onValue: value => Results.Created($"/api/orders/{value}", value),
            onError: errors => errors.ToProblem()
        );
    }

    private static async Task<IResult> GetOrderById(
        [FromRoute] Guid orderId,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new GetOrderByIdQuery(orderId));

        return result.Match(
            onValue: value => Results.Ok(value),
            onError: errors => errors.ToProblem()
        );
    }

    private static async Task<IResult> GetOrders(
        [AsParameters] GetOrdersQuery query,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(query);

        return result.Match(
            onValue: value => Results.Ok(value),
            onError: errors => errors.ToProblem()
        );
    }

    private static async Task<IResult> GetOrdersByCustomer(
        [FromRoute] Guid customerId,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new GetOrdersByCustomerQuery(customerId));

        return result.Match(
            onValue: value => Results.Ok(value),
            onError: errors => errors.ToProblem()
        );
    }

    private static async Task<IResult> IsProductEligibleForFeedback(
        [FromQuery] Guid customerId,
        [FromQuery] Guid productId,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new IsProductEligibleForFeedbackQuery(customerId, productId));

        return result.Match(
            onValue: value => Results.Ok(value),
            onError: errors => errors.ToProblem()
        );
    }
    
     private static async Task<IResult> AddOrderItem(
        [FromRoute] Guid orderId,
        [FromBody] CreateItemDto request,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new AddOrderItemCommand(orderId, request.ProductId, request.ProductName, request.Quantity));

        return result.Match(
            onValue: _ => Results.NoContent(),
            onError: errors => errors.ToProblem());
    }

    private static async Task<IResult> UpdateOrderItem(
        [FromRoute] Guid orderId,
        [FromRoute] Guid itemId,
        [FromBody] UpdateItemRequest request,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new UpdateOrderItemCommand(orderId, itemId, request.Quantity));

        return result.Match(
            onValue: _ => Results.NoContent(),
            onError: errors => errors.ToProblem());
    }

    private static async Task<IResult> RemoveOrderItem(
        [FromRoute] Guid orderId,
        [FromRoute] Guid itemId,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new RemoveOrderItemCommand(orderId, itemId));

        return result.Match(
            onValue: _ => Results.NoContent(),
            onError: errors => errors.ToProblem());
    }
    
    private static async Task<IResult> UpdateOrderItems(
        [FromRoute] Guid orderId,
        [FromBody] List<UpdateItemsDto> request,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new UpdateOrderItemsCommand(orderId, request));

        return result.Match(
            onValue: _ => Results.NoContent(),
            onError: errors => errors.ToProblem());
    }

    private static async Task<IResult> UpdateOrderStatusToProcessing(
        [FromRoute] Guid orderId,
        [FromBody] UpdateStatusRequest request,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new UpdateOrderStatusToProcessingCommand(orderId, request.PaymentId));

        return result.Match(
            onValue: _ => Results.NoContent(),
            onError: errors => errors.ToProblem()
        );
    }

    private static async Task<IResult> UpdateOrderStatusToShipped(
        [FromRoute] Guid orderId,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new UpdateOrderStatusToShippedCommand(orderId));

        return result.Match(
            onValue: _ => Results.NoContent(),
            onError: errors => errors.ToProblem()
        );
    }

    private static async Task<IResult> UpdateOrderStatusToDelivered(
        [FromRoute] Guid orderId,
        [FromBody] UpdateStatusRequest request,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new UpdateOrderStatusToDeliveredCommand(orderId, request.PaymentId));

        return result.Match(
            onValue: _ => Results.NoContent(),
            onError: errors => errors.ToProblem()
        );
    }

    private static async Task<IResult> UpdateOrderStatusToCanceled(
        [FromRoute] Guid orderId,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new UpdateOrderStatusToCanceledCommand(orderId));

        return result.Match(
            onValue: _ => Results.NoContent(),
            onError: errors => errors.ToProblem()
        );
    }

    private static async Task<IResult> UpdateOrderStatusToRefunded(
        [FromRoute] Guid orderId,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new UpdateOrderStatusToRefundedCommand(orderId));

        return result.Match(
            onValue: _ => Results.NoContent(),
            onError: errors => errors.ToProblem()
        );
    }
}