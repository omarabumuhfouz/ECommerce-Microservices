using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Api.Contracts.Category;
using ProductService.Application.Features.Categories.Commands.CreateCategory;
using ProductService.Application.Features.Categories.Commands.DeleteCategory;
using ProductService.Application.Features.Categories.Commands.EditCategory;
using ProductService.Application.Features.Categories.Commands.SetCategoryAsActive;
using ProductService.Application.Features.Categories.DTOs;
using ProductService.Application.Features.Categories.Queries.GetCategories;
using ProductService.Application.Features.Categories.Queries.GetCategoryById;
using ProductService.Application.Features.Categories.Queries.GetProductsByGategory;
using ProductService.Application.Features.Products.DTOs;
using SharedKernel.Extensions;
using SharedKernel.Constants; 

namespace ProductService.Api.Endpoints;

public static class CategoriesEndpoints
{
    public static RouteGroupBuilder MapCategoriesEndpoints(this IEndpointRouteBuilder app)
    {
        var categoriesApi = app.MapGroup("/api/categories")
            .WithTags("Categories")
            .WithOpenApi();

        categoriesApi.MapPost("/", CreateCategory)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly) 
            .Accepts<CreateCategoryCommand>("application/json")
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a new category")
            .WithName("CreateCategory");

        categoriesApi.MapPut("/{categoryId:Guid}", EditCategory)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly) 
            .Accepts<EditCategoryRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Edit an existing category")
            .WithName("EditCategory");

        categoriesApi.MapPut("/{categoryId:Guid}/set-active", SetCategoryAsActive)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Set a category as active")
            .WithName("SetCategoryAsActive");

        categoriesApi.MapDelete("/{categoryId:Guid}", DeleteCategory)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly) 
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a category")
            .WithName("DeleteCategory");

        
        categoriesApi.MapGet("/{categoryId:Guid}", GetCategoryById)
            .Produces<CategoryDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get category by ID")
            .WithName("GetCategoryById");

        categoriesApi.MapGet("/", GetCategories)
            .Produces<List<CategoryDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get all categories")
            .WithName("GetCategories");

        categoriesApi.MapGet("/{categoryId:Guid}/products", GetProductsByCategory)
            .Produces<List<ProductDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get products by category")
            .WithName("GetProductsByCategory");

        return categoriesApi;
    }

    private static async Task<IResult> CreateCategory([FromBody] CreateCategoryCommand command, [FromServices] ISender sender)
    {
        var result = await sender.Send(command);
        return result.Match(v => Results.Created($"/api/categories/{v}", v), e => e.ToProblem());
    }

    private static async Task<IResult> EditCategory([FromRoute] Guid categoryId, [FromBody] EditCategoryRequest request, [FromServices] ISender sender)
    {
        var result = await sender.Send(new EditCategoryCommand(categoryId, request.Name, request.Description));
        return result.Match(_ => Results.NoContent(), e => e.ToProblem());
    }

    private static async Task<IResult> SetCategoryAsActive([FromRoute] Guid categoryId, [FromServices] ISender sender)
    {
        var result = await sender.Send(new SetCategoryAsActiveCommand(categoryId));
        return result.Match(_ => Results.NoContent(), e => e.ToProblem());
    }

    private static async Task<IResult> DeleteCategory([FromRoute] Guid categoryId, [FromServices] ISender sender)
    {
        var result = await sender.Send(new DeleteCategoryCommand(categoryId));
        return result.Match(_ => Results.NoContent(), e => e.ToProblem());
    }

    private static async Task<IResult> GetCategoryById([FromRoute] Guid categoryId, [FromServices] ISender sender)
    {
        var result = await sender.Send(new GetCategoryByIdQuery(categoryId));
        return result.Match(v => Results.Ok(v), e => e.ToProblem());
    }

    private static async Task<IResult> GetCategories([FromServices] ISender sender)
    {
        var result = await sender.Send(new GetCategoriesQuery());
        return result.Match(v => Results.Ok(v), e => e.ToProblem());
    }

    private static async Task<IResult> GetProductsByCategory([FromRoute] Guid categoryId, [FromServices] ISender sender)
    {
        var result = await sender.Send(new GetProductsByCategoryQuery(categoryId));
        return result.Match(v => Results.Ok(v), e => e.ToProblem());
    }
}
