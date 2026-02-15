using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.DTOs;
using PaymentService.Application.Payments.Commands.CompleteCodPayment;
using PaymentService.Application.Payments.Commands.ProcessPayment;
using PaymentService.Application.Payments.Commands.MarkAsCompleted;
using PaymentService.Application.Payments.Commands.MarkAsFailed;
using PaymentService.Application.Payments.Commands.MarkAsPending;
using PaymentService.Application.Payments.Commands.MarkRefundAsComplete;
using PaymentService.Application.Payments.Commands.MarkRefundAsFailed;
using PaymentService.Application.Payments.Commands.ProcessRefund;
using PaymentService.Api.Requests;
using PaymentService.Application.Payments.Queries.GetPaymentById;
using PaymentService.Application.Payments.Queries.GetPaymentByOrderId;
using SharedKernel.Extensions;

namespace PaymentService.Api.Endpoints;

public static class PaymentsEndpoints
{
    public static RouteGroupBuilder MapPaymentsEndpoints(this IEndpointRouteBuilder app)
    {
        var paymentsApi = app.MapGroup("/api/payments")
                .WithTags("Payments")
                .WithOpenApi();

        paymentsApi.MapPost("/", ProcessPayment)
            // .RequireAuthorization(AuthConstants.Policies.Shared) 
            .Accepts<ProcessPaymentCommand>("application/json")
            .Produces<PaymentDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Processes a new payment")
            .WithName("ProcessPayment");

        paymentsApi.MapPost("/refunds", ProcessRefund)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly)
            .Accepts<ProcessRefundRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Processes a refund for an order")
            .WithName("ProcessRefund");

        // 🔍 View Payments (Shared - Customers view their history)
        paymentsApi.MapGet("/{paymentId:Guid}", GetPaymentById)
            // .RequireAuthorization(AuthConstants.Policies.Shared) 
            .Produces<PaymentDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Gets a payment by its ID")
            .WithName("GetPaymentById");

        paymentsApi.MapGet("/order/{orderId:Guid}", GetPaymentByOrderId)
            // .RequireAuthorization(AuthConstants.Policies.Shared)
            .Produces<PaymentDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Gets a payment by its associated order ID")
            .WithName("GetPaymentByOrderId");

        // 🚚 COD Completion (Admin/Staff Only)
        // A customer should not be able to mark their own Cash on Delivery as "Paid"
        paymentsApi.MapPost("/{paymentId:Guid}/order/{orderId}", CompleteCodPayment)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly) 
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Completes a Cash on Delivery (COD) payment")
            .WithName("CompleteCodPayment");

        
        paymentsApi.MapPost("/{paymentId:guid}/complete", MarkPaymentAsCompleted)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly)
            .Accepts<MarkAsCompletedRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Marks a payment as completed")
            .WithName("MarkPaymentAsCompleted");

        paymentsApi.MapPost("/{paymentId:guid}/fail", MarkPaymentAsFailed)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Marks a payment as failed")
            .WithName("MarkPaymentAsFailed");

        paymentsApi.MapPost("/{paymentId:guid}/pend", MarkPaymentAsPending)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Marks a payment as pending")
            .WithName("MarkPaymentAsPending");

        paymentsApi.MapPost("/{paymentId:guid}/refunds/{refundId:guid}/complete", MarkRefundAsComplete)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly)
            .Accepts<CompleteRefundRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Marks a manual refund as complete")
            .WithName("MarkRefundAsComplete");

        paymentsApi.MapPost("/{paymentId:guid}/refunds/{refundId:guid}/fail", MarkRefundAsFailed)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Marks a manual refund as failed")
            .WithName("MarkRefundAsFailed");

        return paymentsApi;
    }

    private static async Task<IResult> ProcessPayment(
        [FromBody] ProcessPaymentCommand command,
        [FromServices] ISender sender) 
    {
        var result = await sender.Send(command);

        return result.Match(
            onValue: value => Results.Ok(value),
            onError: errors => errors.ToProblem()
        );

    }

    private static async Task<IResult> ProcessRefund(
        [FromBody] ProcessRefundRequest request,
        [FromServices] ISender sender)
    {
        var command = new ProcessRefundCommand(
            request.OrderId,
            request.CancellationId,
            request.ApprovedByUserId,
            request.Amount,
            request.Remarks);

        var result = await sender.Send(command);

        return result.Match(
            onValue: _ => Results.NoContent(),
            onError: errors => errors.ToProblem()
        );
    }

    private static async Task<IResult> GetPaymentById(
        [FromRoute] Guid paymentId,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new GetPaymentByIdQuery(paymentId));

        return result.Match(
            onValue: value => Results.Ok(value),
            onError: errors => errors.ToProblem()
        );
    }

    private static async Task<IResult> GetPaymentByOrderId(
        [FromRoute] Guid orderId,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new GetPaymentByOrderIdQuery(orderId));

        return result.Match(
            onValue: value => Results.Ok(value),
            onError: errors => errors.ToProblem()
        );
    }

    private static async Task<IResult> CompleteCodPayment(
        [FromRoute] Guid paymentId,
        [FromRoute] Guid orderId,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new CompleteCodPaymentCommand(paymentId, orderId));

        return result.Match(
            onValue: value => Results.NoContent(),
            onError: errors => errors.ToProblem()
        );
    }

    private static async Task<IResult> MarkPaymentAsCompleted(
        [FromRoute] Guid paymentId,
        [FromBody] MarkAsCompletedRequest request,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new MarkAsCompletedCommand(paymentId, request.TransactionId));

        return result.Match(
            onValue: _ => Results.NoContent(),
            onError: errors => errors.ToProblem()
        );
    }

    private static async Task<IResult> MarkPaymentAsFailed(
        [FromRoute] Guid paymentId,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new MarkAsFailedCommand(paymentId));

        return result.Match(
            onValue: _ => Results.NoContent(),
            onError: errors => errors.ToProblem()
        );
    }

    private static async Task<IResult> MarkPaymentAsPending(
        [FromRoute] Guid paymentId,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new MarkAsPendingCommand(paymentId));

        return result.Match(
            onValue: _ => Results.NoContent(),
            onError: errors => errors.ToProblem()
        );
    }

    private static async Task<IResult> MarkRefundAsComplete(
        [FromRoute] Guid paymentId,
        [FromRoute] Guid refundId,
        [FromBody] CompleteRefundRequest request,
        [FromServices] ISender sender)
    {
        var command = new MarkRefundAsCompleteCommand(paymentId, refundId, request.TransactionId, request.ApprovedBy);
        var result = await sender.Send(command);

        return result.Match(
            onValue: _ => Results.NoContent(),
            onError: errors => errors.ToProblem()
        );
    }

    private static async Task<IResult> MarkRefundAsFailed(
        [FromRoute] Guid paymentId,
        [FromRoute] Guid refundId,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new MarkRefundAsFailedCommand(paymentId, refundId));

        return result.Match(
            onValue: _ => Results.NoContent(),
            onError: errors => errors.ToProblem()
        );
    }
}
