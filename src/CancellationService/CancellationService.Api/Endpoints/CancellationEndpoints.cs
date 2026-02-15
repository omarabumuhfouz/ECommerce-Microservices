using CancellationService.Api.Contracts.Cancellations;
using CancellationService.Application.Cancellations.Commands.ApproveCancellation;
using CancellationService.Application.Cancellations.Commands.CreateCancellation;
using CancellationService.Application.Cancellations.Commands.RejectCancellation;
using CancellationService.Application.Cancellations.DTOs;
using CancellationService.Application.Cancellations.Queries.GetCancellationById;
using CancellationService.Application.Cancellations.Queries.GetCancellations;
using CancellationService.Application.Cancellations.Queries.GetCancellationByOrderId;
using CancellationService.Application.Cancellations.Commands.UpdateCancellationReason;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Common;
using SharedKernel.Extensions;
using CancellationService.Domain.Cancellations.Enums;

namespace CancellationService.Api.Endpoints;

public static class CancellationEndpoints
{
    private static readonly Guid TestingAdminId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static IEndpointRouteBuilder MapCancellationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/cancellations")
            .WithTags("Cancellations")
            .WithOpenApi();

        // ✋ Request Cancellation (Shared - Customers initiate this)
        group.MapPost("/", CreateCancellation)
            // .RequireAuthorization(AuthConstants.Policies.Shared) 
            .WithName("CreateCancellation")
            .WithSummary("Creates a new cancellation request")
            .WithDescription("Validates the order eligibility and creates a pending cancellation.")
            .Produces<CancellationDto>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // ✅ Approve (Admin Only)
        group.MapPut("/{id:guid}/approve", ApproveCancellation)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly) 
            .WithName("ApproveCancellation")
            .WithSummary("Approves a cancellation request")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // ❌ Reject (Admin Only)
        group.MapPut("/{id:guid}/reject", RejectCancellation)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly) 
            .WithName("RejectCancellation")
            .WithSummary("Rejects a cancellation request")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // 📝 Update Reason (Admin Only)
        group.MapPut("/{id:guid}/reason", UpdateCancellationReason)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly) 
            .WithName("UpdateCancellationReason")
            .WithSummary("Updates the reason for a cancellation request")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // 🔍 Get Single (Shared - User views own, Admin views any)
        group.MapGet("/{id:guid}", GetCancellationById)
            // .RequireAuthorization(AuthConstants.Policies.Shared) 
            .WithName("GetCancellationById")
            .WithSummary("Gets a cancellation by ID")
            .Produces<CancellationDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // 🔍 Get by Order ID
        group.MapGet("/order/{orderId:guid}", GetCancellationByOrderId)
            // .RequireAuthorization(AuthConstants.Policies.Shared)
            .WithName("GetCancellationByOrderId")
            .WithSummary("Gets a cancellation by Order ID")
            .Produces<CancellationDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // � List All (Admin Only - Dashboard)
        group.MapGet("/", GetCancellations)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly) 
            .WithName("GetCancellations")
            .WithSummary("Gets a paginated list of cancellations")
            .Produces<PagedList<CancellationDto>>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return app;
    }

    public static async Task<IResult> CreateCancellation(
        [FromBody] CreateCancellationRequest request,
        ISender sender)
    {

        var result = await sender.Send(new CreateCancellationCommand(
                request.OrderId,
                request.Reason
        ));

        return result.Match(
            onValue: dto => Results.Created($"/api/cancellations/{dto.Id}", dto),
            onError: errors => errors.ToProblem()
        );
    }

    public static async Task<IResult> ApproveCancellation(
        [FromRoute] Guid id,
        [FromBody] ApproveCancellationRequest request,
        [FromServices] ISender sender,
         HttpContext context

    )
    {
        var result = await sender.Send(new ApproveCancellationCommand(id, request.Remarks, request.Charges, TestingAdminId));
        return result.Match(
            onValue: value => Results.NoContent(),
            onError: errors => errors.ToProblem()
        );
    }

    public static async Task<IResult> RejectCancellation(
        [FromRoute] Guid id,
        [FromBody] RejectCancellationRequest request,
        ISender sender,
            HttpContext context

    )
    {
        var result = await sender.Send(new RejectCancellationCommand(id, request.Remarks, TestingAdminId));

        return result.Match(
            onValue: value => Results.NoContent(),
            onError: errors => errors.ToProblem()
        );
    }

    public static async Task<IResult> UpdateCancellationReason(
        [FromRoute] Guid id,
        [FromBody] UpdateCancellationReasonRequest request,
        ISender sender
    )
    {
        var result = await sender.Send(new UpdateCancellationReasonCommand(id, request.Reason));

        return result.Match(
            onValue: value => Results.NoContent(),
            onError: errors => errors.ToProblem()
        );
    }

    public static async Task<IResult> GetCancellationById(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetCancellationByIdQuery(id));

        return result.Match(
            onValue: value => Results.Ok(value),
            onError: errors => errors.ToProblem()
        );
    }

    public static async Task<IResult> GetCancellationByOrderId(
        Guid orderId,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetCancellationByOrderIdQuery(orderId));

        return result.Match(
            onValue: value => Results.Ok(value),
            onError: errors => errors.ToProblem()
        );
    }

    public static async Task<IResult> GetCancellations(
        [AsParameters] GetCancellationsRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        CancellationStatus? statusEnum = null;
        if (Enum.TryParse<CancellationStatus>(request.Status, true, out var enumResult)) statusEnum = enumResult;

        var query = new GetCancellationsQuery(
                request.PageNumber,
                request.PageSize,
                request.SearchTerm,
                statusEnum,
                request.SortBy,
                request.IsAscending);

        var result = await sender.Send(query);

        return result.Match(
                onValue: value => Results.Ok(value),
                onError: errors => errors.ToProblem()
        );
    }
}