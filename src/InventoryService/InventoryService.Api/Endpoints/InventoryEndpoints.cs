#region Usings
using InventoryService.Api.Contracts;
using InventoryService.Application.Features.Inventory.Commands.AdjustInventory;
using InventoryService.Application.Features.Inventory.Commands.Create;
using InventoryService.Application.Features.Inventory.Commands.DispatchStock;
using InventoryService.Application.Features.Inventory.Commands.ReceiveStock;
using InventoryService.Application.Features.Inventory.Commands.ReleaseStock;
using InventoryService.Application.Features.Inventory.Commands.ReserveStock;
using InventoryService.Application.Features.Inventory.Commands.UpdateLowStockThreshold;
using InventoryService.Application.Features.Inventory.Queries.GetActiveReservations;
using InventoryService.Application.Features.Inventory.Queries.GetBatchStockAvailability;
using InventoryService.Application.Features.Inventory.Queries.GetLowStockReport;
using InventoryService.Application.Features.Inventory.Queries.GetStockAvailability;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Extensions;

#endregion

namespace InventoryService.Api.Endpoints;

public static class InventoryEndpoints
{
    public static RouteGroupBuilder MapInventoryEndpoints(this IEndpointRouteBuilder app)
    {
        var inventoriesApi = app.MapGroup("/api/inventories")
                            .WithTags("Inventories")
                            .WithOpenApi();

        inventoriesApi.MapPost("/", CreateInventory)
            .WithSummary("Create Inventory")
            .WithDescription("Creates a new inventory record for a product.")
            .WithName("CreateInventory")
            .Produces<InventoryCreatedResponse>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        inventoriesApi.MapPatch("/{productId}/adjust", AdjustInventory)
            .WithSummary("Adjust Inventory Stock")
            .WithDescription("Manually adjusts the stock quantity for a product.")
            .WithName("AdjustInventory")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        inventoriesApi.MapPatch("/{productId}/dispatch", DispatchStock)
            .WithSummary("Dispatch Stock")
            .WithDescription("Reduces stock for a dispatched/shipped product.")
            .WithName("DispatchStock")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        inventoriesApi.MapPatch("/{productId}/receive", ReceiveStock)
            .WithSummary("Receive Stock")
            .WithDescription("Increases stock for a received product shipment.")
            .WithName("ReceiveStock")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        inventoriesApi.MapPost("/{productId}/reserve-stock", ReserveStock)
            .WithSummary("Reserve Stock")
            .WithDescription("Reserves a specified quantity of a product for an order.")
            .WithName("ReserveStock")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        inventoriesApi.MapPost("/{productId}/release-stock", ReleaseStock)
            .WithSummary("Release Stock Reservation")
            .WithDescription("Releases a previously made stock reservation for an order.")
            .WithName("ReleaseStock")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        inventoriesApi.MapPatch("/{productId}/threshold", UpdateLowStockThreshold)
            .WithSummary("Update Low Stock Threshold")
            .WithDescription("Updates the low stock threshold configuration for a specific product inventory.")
            .WithName("UpdateLowStockThreshold")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        inventoriesApi.MapGet("/{productId}/reservations", GetActiveReservations)
            .WithSummary("Get Active Reservations")
            .WithDescription("Retrieves a list of active stock reservations for a specific product.")
            .WithName("GetActiveReservations")
            .Produces<List<StockReservationDto>>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        inventoriesApi.MapPost("/stock/availability-batch", GetBatchStockAvailability)
            .WithSummary("Get Batch Stock Availability")
            .WithDescription("Checks the stock availability for a batch of products.")
            .WithName("GetBatchStockAvailability")
            .Produces<List<InventoryAvailabilityDto>>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        inventoriesApi.MapGet("/reports/low-stock", GetLowStockReport)
            .WithSummary("Get Low Stock Report")
            .WithDescription("Retrieves a report of all inventory items that are below their low stock threshold.")
            .WithName("GetLowStockReport")
            .Produces<List<LowStockItemDto>>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        inventoriesApi.MapGet("/{productId}/stock/availability", GetStockAvailability)
            .WithSummary("Get Stock Availability")
            .WithDescription("Checks the stock availability for a single product.")
            .WithName("GetStockAvailability")
            .Produces<InventoryAvailabilityDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return inventoriesApi;
    }

    public static async Task<IResult> UpdateLowStockThreshold(
        [FromRoute] Guid productId,
        [FromBody] UpdateLowStockThresholdInventoryRequest request,
        [FromServices] ISender sender,
        CancellationToken ct)
    {
        var command = new UpdateLowStockThresholdCommand(productId, request.NewThreshold);

        var result = await sender.Send(command, ct);

        return result.Match(
            onValue: _ => Results.NoContent(),
            onError: errors => errors.ToProblem()
        );
    }

    public static async Task<IResult> CreateInventory(
        [FromBody] CreateInventoryRequest request,
        [FromServices] ISender sender,
        CancellationToken ct)
    {
        var command = new CreateInventoryCommand(request.ProductId, request.InitialStock, request.LowStockThreshold);
        var result = await sender.Send(command, ct);

        // Assuming a "GetInventoryById" endpoint exists for the CreatedAtRoute response
        return result.Match(
            onValue: id => Results.CreatedAtRoute("GetInventoryById", new { inventoryId = id }, new InventoryCreatedResponse(id)),
            onError: errors => errors.ToProblem()
        );
    }

    public static async Task<IResult> AdjustInventory(
        [FromRoute] Guid productId,
        [FromBody] AdjustInventoryRequest request,
        [FromServices] ISender sender,
        CancellationToken ct)
    {
        var command = new AdjustInventoryCommand(productId,request.OrderId, request.Quantity);
        var result = await sender.Send(command, ct);

        return result.Match(
            onValue: _ => Results.NoContent(),
            onError: errors => errors.ToProblem()
        );
    }

    public static async Task<IResult> DispatchStock(
        [FromRoute] Guid productId,
        [FromBody] DispatchStockRequest request,
        [FromServices] ISender sender,
        CancellationToken ct)
    {
        var command = new DispatchStockCommand(productId, request.OrderId);
        var result = await sender.Send(command, ct);

        return result.Match(
            onValue: _ => Results.NoContent(),
            onError: errors => errors.ToProblem()
        );
    }

    public static async Task<IResult> ReceiveStock(
        [FromRoute] Guid productId,
        [FromBody] ReceiveStockRequest request,
        [FromServices] ISender sender,
        CancellationToken ct)
    {
        var command = new ReceiveStockCommand(productId, request.Quantity);
        var result = await sender.Send(command, ct);

        return result.Match(
            onValue: _ => Results.NoContent(),
            onError: errors => errors.ToProblem()
        );
    }

    public static async Task<IResult> ReserveStock(
        [FromRoute] Guid productId,
        [FromBody] ReserveStockRequest request,
        [FromServices] ISender sender,
        CancellationToken ct)
    {
        var command = new ReserveStockCommand(productId, request.OrderId, request.Quantity);
        var result = await sender.Send(command, ct);

        return result.Match(
            onValue: _ => Results.NoContent(),
            onError: errors => errors.ToProblem()
        );
    }

    public static async Task<IResult> ReleaseStock(
        [FromRoute] Guid productId,
        [FromBody] ReleaseStockRequest request,
        [FromServices] ISender sender,
        CancellationToken ct)
    {
        var command = new ReleaseStockCommand(productId, request.OrderId);
        var result = await sender.Send(command, ct);

        return result.Match(
            onValue: _ => Results.NoContent(),
            onError: errors => errors.ToProblem()
        );
    }

    public static async Task<IResult> GetActiveReservations(
        [FromRoute] Guid productId,
        [FromServices] ISender sender,
        CancellationToken ct)
    {
        var query = new GetActiveReservationsQuery(productId);
        var result = await sender.Send(query, ct);

        return result.Match(
            onValue: Results.Ok,
            onError: errors => errors.ToProblem()
        );
    }

    public static async Task<IResult> GetBatchStockAvailability(
        [FromBody] GetBatchStockAvailabilityQuery query,
        [FromServices] ISender sender,
        CancellationToken ct)
    {
        var result = await sender.Send(query, ct);

        return result.Match(
            onValue: Results.Ok,
            onError: errors => errors.ToProblem()
        );
    }

    public static async Task<IResult> GetLowStockReport(
        [FromServices] ISender sender,
        CancellationToken ct)
    {
        var query = new GetLowStockReportQuery();
        var result = await sender.Send(query, ct);

        return result.Match(
            onValue: Results.Ok,
            onError: errors => errors.ToProblem()
        );
    }

    public static async Task<IResult> GetStockAvailability(
        [FromRoute] Guid productId,
        [FromQuery] int requestedQuantity,
        [FromServices] ISender sender,
        CancellationToken ct)
    {
        var query = new GetStockAvailabilityQuery(productId, requestedQuantity);
        var result = await sender.Send(query, ct);

        return result.Match(
            onValue: Results.Ok,
            onError: errors => errors.ToProblem()
        );
    }
}