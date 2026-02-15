using MediatR;
using Microsoft.AspNetCore.Mvc;
using FeedbackService.Application.Feedbacks.Commands.DeleteFeedback;
using FeedbackService.Application.Feedbacks.Commands.SubmitFeedback;
using FeedbackService.Application.Feedbacks.Commands.UpdateFeedback;
using FeedbackService.Application.Feedbacks.Queries.GetFeedbackForProduct;
using FeedbackService.Application.Feedbacks.Queries.GetFeedbacks;
using FeedbackService.Application.Feedbacks.DTOs;
using FeedbackService.Api.Contracts; 
using SharedKernel.Extensions;
using SharedKernel.Constants; 

namespace FeedbackService.Api.Endpoints;

public static class FeedbackEndpoints
{
    public static void MapFeedbacksEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/feedbacks")
            .WithTags("Feedbacks")
            .WithOpenApi();

        group.MapGet("/", GetFeedbacks)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly) 
            .WithName("GetFeedbacks")
            .WithSummary("Retrieves all feedbacks in the system")
            .Produces<List<FeedbackDto>>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        group.MapGet("/product/{productId:guid}", GetFeedbackForProduct)
            .WithName("GetFeedbacksByProduct")
            .WithSummary("Retrieves aggregate feedback and comments for a specific product")
            .Produces<ProductFeedbackDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapPost("/", SubmitFeedback)
            // .RequireAuthorization(AuthConstants.Policies.CustomerOnly) 
            .Accepts<SubmitFeedbackRequest>("application/json")
            .Produces<FeedbackDto>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .WithName("SubmitFeedback")
            .WithSummary("Submits new feedback for a product");
            
        group.MapPut("/{id:guid}", UpdateFeedback)
            // .RequireAuthorization(AuthConstants.Policies.CustomerOnly) 
            .Accepts<UpdateFeedbackRequest>("application/json")
            .WithName("UpdateFeedback") 
            .WithSummary("Updates an existing feedback comment or rating") 
            .Produces<FeedbackDto>(StatusCodes.Status200OK) 
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id:guid}/customer/{customerId:guid}", DeleteFeedback)
            // .RequireAuthorization(AuthConstants.Policies.CustomerOnly) 
            .WithName("DeleteFeedback")
            .WithSummary("Deletes a specific feedback entry")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);
    }


    public static async Task<IResult> GetFeedbacks(
        ISender sender, 
        CancellationToken ct)
    {
        var query = new GetFeedbacksQuery();
        var result = await sender.Send(query, ct);

        return result.Match(
            onValue: dtos => Results.Ok(dtos),
            onError: errors => errors.ToProblem()
        );
    }

    public static async Task<IResult> GetFeedbackForProduct(
        Guid productId, 
        ISender sender, 
        CancellationToken ct)
    {
        var query = new GetFeedbackForProductQuery(ProductId: productId);
        var result = await sender.Send(query, ct);

        return result.Match(
            onValue: dto => Results.Ok(dto),
            onError: errors => errors.ToProblem()
        );
    }

    public static async Task<IResult> SubmitFeedback(
        [FromBody] SubmitFeedbackRequest request,
        ISender sender, 
        CancellationToken ct)
    {

        var command = new SubmitFeedbackCommand(
            request.CustomerId,
            request.ProductId,
            request.CustomerName,
            request.ProductName,
            request.Rating,
            request.Comment);
        var result = await sender.Send(command, ct);

        return result.Match(
            onValue: dto => Results.Created($"/api/feedbacks/{dto.Id}", dto), 
            onError: errors => errors.ToProblem()
        );
    }

    public static async Task<IResult> UpdateFeedback(
        [FromRoute] Guid id,
        [FromBody] UpdateFeedbackRequest request, 
        ISender sender,
        CancellationToken ct)
    {
        // The Command Handler must check if this customerId matches the feedback owner
        var command = new UpdateFeedbackCommand(id, request.CustomerId, request.Rating, request.Comment);
        var result = await sender.Send(command, ct);

        return result.Match(
            onValue: dto => Results.Ok(dto),
            onError: errors => errors.ToProblem()
        );
    }

    public static async Task<IResult> DeleteFeedback(
        Guid id, 
        Guid customerId,
        ISender sender, 
        CancellationToken ct)
    {

        var command = new DeleteFeedbackCommand(FeedbackId: id, CustomerId: customerId);
        var result = await sender.Send(command, ct);

        return result.Match(
            onValue: _ => Results.NoContent(),
            onError: errors => errors.ToProblem()
        );
    }
}