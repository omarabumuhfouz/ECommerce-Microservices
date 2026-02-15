using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Extensions;
using ShoppingCartService.Api.Contracts.CartRequest;
using ShoppingCartService.Application.Carts.Commands.AddToCart;
using ShoppingCartService.Application.Carts.Commands.CheckoutCart;
using ShoppingCartService.Application.Carts.Commands.ClearCart;
using ShoppingCartService.Application.Carts.Commands.EditCartItem;
using ShoppingCartService.Application.Carts.Commands.RemoveCartItem;
using ShoppingCartService.Application.Carts.DTOs;
using ShoppingCartService.Application.Carts.Queries.GetCartByCustomer;

namespace ShoppingCartService.Api.Endpoints;

public static class CartsEndpoints
{
    public static RouteGroupBuilder MapCartsEndpoints(this IEndpointRouteBuilder app)
    {
        var cartsApi = app.MapGroup("/api/carts")
                          .WithTags("Carts")
                          .WithOpenApi();


        cartsApi.MapGet("/customer/{customerId:Guid}", GetCartByCustomer)
        //    .RequireAuthorization(AuthConstants.Policies.Shared) 
           .Produces<CartDto>(StatusCodes.Status200OK)
           .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
           .WithSummary("Get customer's shopping cart")
           .WithName("GetCartByCustomerId");


        cartsApi.MapPost("/", AddToCart)
            // .RequireAuthorization(AuthConstants.Policies.Shared) 
            .Accepts<AddToCartRequest>("application/json")
            .Produces<CartDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Add an item to the cart")
            .WithName("AddToCart");

        cartsApi.MapPut("/items/{cartItemId:Guid}/quantity", EditCartItem)
            // .RequireAuthorization(AuthConstants.Policies.Shared) 
            .Accepts<EditCartItemRequest>("application/json")
            .Produces<CartDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Update an item's quantity in the cart")
            .WithName("EditCartItem");

        cartsApi.MapDelete("/items/{cartItemId:Guid}", RemoveCartItem)
            // .RequireAuthorization(AuthConstants.Policies.Shared) 
            .Produces<CartDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Remove an item from the cart")
            .WithName("RemoveCartItem");

        cartsApi.MapDelete("/clear", ClearCart)
            // .RequireAuthorization(AuthConstants.Policies.Shared) 
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Clear all items from the cart")
            .WithName("ClearCart");

        cartsApi.MapPost("/checkout", CheckoutCart)
            // .RequireAuthorization(AuthConstants.Policies.Shared)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
            .WithSummary("Checkout the cart")
            .WithName("CheckoutCart");

        return cartsApi;
    }

    private static async Task<IResult> GetCartByCustomer(
        [FromRoute] Guid customerId,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new GetCartByCustomerIdQuery(customerId));

        return result.Match(
            onValue: value => Results.Ok(value),
            onError: error => error.ToProblem()
        );
    }

    private static async Task<IResult> AddToCart(
        [FromBody] AddToCartRequest request,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new AddToCartCommand(
            request.CustomerId,
            request.ProductId,
            request.Quantity));

        return result.Match(
            onValue: value => Results.Ok(value),
            onError: error => error.ToProblem()
        );
    }

    private static async Task<IResult> EditCartItem(
        [FromRoute] Guid cartItemId,
        [FromBody] EditCartItemRequest request,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new EditCartItemCommand (
            request.CustomerId,
            cartItemId,
            request.Quantity));

        return result.Match(
            onValue: value => Results.Ok(value),
            onError: error => error.ToProblem()
        );
    }

    private static async Task<IResult> RemoveCartItem(
        [FromQuery] Guid customerId,
        [FromRoute] Guid cartItemId, 
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new RemoveCartItemCommand(
            customerId,
            cartItemId));

        return result.Match(
            onValue: value => Results.Ok(value),
            onError: error => error.ToProblem()
        );
    }

    private static async Task<IResult> ClearCart(
        [FromQuery] Guid customerId,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new ClearCartCommand(
            customerId));

        return result.Match(
            onValue: value => Results.NoContent(),
            onError: error => error.ToProblem()
        );
    }

    private static async Task<IResult> CheckoutCart(
        [FromHeader] string requestId,
        [FromQuery] Guid customerId,
        [FromServices] ISender sender)
    {
        if(!Guid.TryParse(requestId, out var requestIdGuid)) return Results.BadRequest("Invalid or missing RequestId header.");

        var result = await sender.Send(new CheckoutCartCommand
        (
            requestIdGuid,
            customerId
      ));


        return result.Match(
            onValue: value => Results.NoContent(),
            onError: error => error.ToProblem()
        );
    }
}