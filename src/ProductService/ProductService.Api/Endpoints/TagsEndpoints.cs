using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Api.Contracts.Tags;
using ProductService.Application.DTOs;
using ProductService.Application.Features.Tags.Commands.AddTag;
using ProductService.Application.Features.Tags.Commands.DeleteTag;
using ProductService.Application.Features.Tags.Commands.EditTag;
using ProductService.Application.Features.Tags.Commands.RestoreTag;
using ProductService.Application.Features.Tags.Queries.GetTags;
using SharedKernel.Extensions;

namespace ProductService.Api.Endpoints;


public static class TagsEndpoints
{
    public static RouteGroupBuilder MapTagsEndpoints(this IEndpointRouteBuilder app)
    {
        var tagsApi = app.MapGroup("/api/tags")
                .WithTags("Tags")
                .WithOpenApi();

        tagsApi.MapPost("/", AddTag)
            .Accepts<AddTagCommand>("application/json")
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a new tag")
            .WithDescription("Adds a new tag to the system. The tag name must be provided in the request body.")
            .WithName("AddTag");

        tagsApi.MapDelete("/{tagId:Guid}", DeleteTag)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete an existing tag")
            .WithDescription("Removes a tag from the system based on its ID. Fails with a 409 Conflict if the tag is still associated with products.")
            .WithName("DeleteTag");

        tagsApi.MapPut("/{tagId:Guid}", RestoreTag)
                    // .RequireAuthorization(AuthConstants.Policies.AdminOnly) 
                    .Produces(StatusCodes.Status204NoContent)
                    .ProducesProblem(StatusCodes.Status404NotFound)
                    .ProducesProblem(StatusCodes.Status500InternalServerError)
                    .WithSummary("Restore Deleted  tag")
                    .WithName("RestoreTag");


        tagsApi.MapPut("/{tagId:Guid}", EditTag)
            .Accepts<UpdateTagRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Update an existing tag")
            .WithDescription("Updates the name of an existing tag based on its ID. Fails with a 409 Conflict if the new name already exists.")
            .WithName("EditTag");

        tagsApi.MapGet("/", GetTags)
            .Produces<List<TagDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get all tags")
            .WithDescription("Retrieves a complete list of all tags currently in the system.")
            .WithName("GetTags");
        
        return tagsApi;
    }

    private static async Task<IResult> AddTag(
        [FromBody] AddTagCommand command,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(command);

        return result.Match(
            onValue: value => Results.Created("/api/tags/{id}",value),
            onError: error => error.ToProblem()
        );
    }

    private static async Task<IResult> DeleteTag(
        [FromRoute] Guid tagId,
         [FromServices] ISender sender)
    {
        var result = await sender.Send(new DeleteTagCommand(tagId));

        return result.Match(
            onValue: value => Results.NoContent(),
            onError: error => error.ToProblem()
        );
    }

private static async Task<IResult> RestoreTag(
        [FromRoute] Guid tagId,
        [FromServices] ISender sender
    )
    {
        var result = await sender.Send(new RestoreTagCommand(tagId));
        return result.Match(_ => Results.NoContent(), e => e.ToProblem());
    }

    private static async Task<IResult> EditTag(
        [FromRoute] Guid tagId,
        [FromBody] UpdateTagRequest request,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new EditTagCommand(tagId, request.Name));

        return result.Match(
            onValue: value => Results.NoContent(),
            onError: error => error.ToProblem()
        );
    }


    private static async Task<IResult> GetTags(
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new GetTagsQuery());

        return result.Match(
            onValue: value => Results.Ok(value),
            onError: error => error.ToProblem()
        );
    }


}

