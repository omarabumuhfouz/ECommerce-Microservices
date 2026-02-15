#region Usings
    
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Api.Contracts.Product;
using ProductService.Application.Features.Products.Commands.AddDiscount;
using ProductService.Application.Features.Products.Commands.CreateProduct;
using ProductService.Application.Features.Products.Commands.DeleteDiscount;
using ProductService.Application.Features.Products.Commands.DeleteProduct;
using ProductService.Application.Features.Products.Commands.EditPrice;
using ProductService.Application.Features.Products.Commands.EditProduct;
using ProductService.Application.Features.Products.Commands.EditProductStatus;
using ProductService.Application.Features.Products.Commands.EditStock;
using ProductService.Application.Features.Products.Commands.Features.AddFeatures;
using ProductService.Application.Features.Products.Commands.Features.DeleteFeature;
using ProductService.Application.Features.Products.Commands.Features.EditFeature;
using ProductService.Application.Features.Products.Commands.ImagesManagement.AddRelatedImages;
using ProductService.Application.Features.Products.Commands.ImagesManagement.DeleteRelatedImage;
using ProductService.Application.Features.Products.Commands.ImagesManagement.ReplaceMainImage;
using ProductService.Application.Features.Products.Commands.ImagesManagement.ReplaceRelatedImage;
using ProductService.Application.Features.Products.Commands.Tags.AddTag;
using ProductService.Application.Features.Products.Commands.Tags.DeleteTag;
using ProductService.Application.Features.Products.DTOs;
using ProductService.Application.Features.Products.Queries.GetProductById;
using ProductService.Application.Features.Products.Queries.GetProductNames;
using ProductService.Application.Features.Products.Queries.GetProducts;
using SharedKernel.Common;
using SharedKernel.Extensions;
using SharedKernel.Constants; // 👈 1. Add SharedKernel for AuthConstants
# endregion

namespace ProductService.Api.Endpoints;


public static class ProductsEndpoints
{
    public static RouteGroupBuilder MapProductsEndpoints(this IEndpointRouteBuilder app)
    {
        var productsApi = app.MapGroup("/api/products")
                .WithTags("Products")
                .WithOpenApi();

        #region Main Product Endpoints

        productsApi.MapPost("/", CreateProduct)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly) 
            .Accepts<CreateProductCommand>("application/json")
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Create a new product")
            .WithName("Create Product");

        productsApi.MapPut("/{productId:Guid}", EditProduct)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly) 
            .Accepts<EditProductRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Edit an existing product")
            .WithName("Edit Product");

        productsApi.MapPut("/{productId:Guid}/status", EditProductStatus)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly) 
            .Accepts<EditProductStatusRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Edit product availability status")
            .WithName("Edit Product Status");

        productsApi.MapDelete("/{productId:Guid}", DeleteProduct)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly) 
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a product")
            .WithName("Delete Product");

        productsApi.MapGet("/{productId:Guid}", GetProductById)
            .Produces<ProductDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get product by ID")
            .WithName("Get Product By Id");

        productsApi.MapGet("/names", GetProductNamesByIds)
            .Accepts<GetProductNamesQuery>("application/json")
            .Produces<List<ProductNameDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get product names by IDs")
            .WithName("Get Product Names By Ids");

        productsApi.MapGet("/", GetProducts)
            .Accepts<GetProductsRequest>("application/json")
            .Produces<PagedList<ProductDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get all products")
            .WithName("Get Products");

        #endregion

        #region  Product Image Endpoints (Admin Only)

        productsApi.MapPost("/{productId:Guid}/related-images", AddRelatedImage)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly) 
            .Accepts<List<ImageDto>>("application/json")
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Upload related images for a product")
            .WithName("AddRelatedImages");

        productsApi.MapDelete("/{productId:Guid}/related-images", DeleteRelatedImage)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly) 
            .Accepts<DeleteRelatedImageRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete related images for a product")
            .WithName("DeleteRelatedImages");

        productsApi.MapPut("/{productId:Guid}/related-images", EditRelatedImage)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly) 
            .Accepts<EditRelatedImageRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Update related images for a product")
            .WithName("UpdateRelatedImages");

        productsApi.MapPut("/{productId:Guid}/main-image", EditMainImage)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly) 
            .Accepts<EditMainImageRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Update the main image for a product")
            .WithName("UpdateMainImage");

        #endregion

        #region  Product Tag Endpoints (Admin Only)

        productsApi.MapPost("/{productId:Guid}/tags/{tagId:Guid}", AddTagToProduct)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly) 
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Add a tag to a product")
            .WithName("AddTagToProduct");

        productsApi.MapDelete("/{productId:Guid}/tags/{tagId:Guid}", DeleteTagFromProduct)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly) 
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Remove a tag from a product")
            .WithName("DeleteTagFromProduct");

        #endregion

        #region  Product Stock Endpoints (Admin Only)

        productsApi.MapPut("{productId:Guid}/stock", EditStock)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly) 
            .Accepts<EditStockRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Update product stock level")
            .WithName("EditStock");

        productsApi.MapPut("{productId:Guid}/price", EditPrice)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly) 
            .Accepts<EditPriceRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Update a product's price")
            .WithName("EditPrice");

        #endregion

        #region  Product Discount Endpoints (Admin Only)

        productsApi.MapPost("/{productId:Guid}/discount", AddDiscount)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly) 
            .Accepts<AddDiscountRequest>("application/json")
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Add a discount to a product")
            .WithName("AddProductDiscount");

        productsApi.MapDelete("/{productId:Guid}/discount", DeleteDiscount)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly) 
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Remove a discount from a product")
            .WithName("DeleteProductDiscount");


        #endregion

        #region Product Feature Endpoints (Admin Only)

        productsApi.MapPost("/{productId:Guid}/features", AddFeatures)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly) 
            .Accepts<List<FeatureDto>>("application/json")
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Add features to a product")
            .WithName("AddFeatures");

        productsApi.MapDelete("/{productId:Guid}/features", DeleteFeature)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly) 
            .Accepts<DeleteFeatureRequest>("application/json") 
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Remove a feature from a product")
            .WithName("DeleteFeature");

        productsApi.MapPut("/{productId:Guid}/features", EditFeature)
            // .RequireAuthorization(AuthConstants.Policies.AdminOnly) 
            .Accepts<EditFeatureRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Update a product feature")
            .WithName("EditFeature");

        #endregion

        return productsApi;
    }


    #region  Product Tag Method

    private static async Task<IResult> AddTagToProduct(
        [FromRoute] Guid tagId,
        [FromRoute] Guid productId,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new AddTagToProductCommand(tagId, productId));

        return result.Match(
            onValue: value => Results.Created($"/api/products/{productId}/tags/{tagId}", value),
            onError: error => error.ToProblem()
        );
    }

    private static async Task<IResult> DeleteTagFromProduct(
        [FromRoute] Guid productId,
        [FromRoute] Guid tagId,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new DeleteTagFromProductCommand(tagId, productId));

        return result.Match(
            onValue: value => Results.NoContent(),
            onError: error => error.ToProblem()
        );

    }

    #endregion

    #region  Product Stock Method

    private static async Task<IResult> EditStock(
        [FromRoute] Guid productId,
        [FromBody] EditStockRequest request,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new EditStockCommand(productId, request.Quantity, request.Operation));

        return result.Match(
            onValue: value => Results.NoContent(),
            onError: error => error.ToProblem()
        );
    }
    
     private static async Task<IResult> EditPrice(
        [FromRoute] Guid productId,
        [FromBody] EditPriceRequest request,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new EditPriceCommand(productId, request.Price, request.Currency));

        return result.Match(
            onValue: value => Results.NoContent(),
            onError: error => error.ToProblem()
        );
    }

    #endregion

    #region Product Image Methods
    private static async Task<IResult> AddRelatedImage(
        [FromRoute] Guid productId,
        [FromBody] List<ImageDto> relatedImages,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new AddRelatedImagesToProductCommand(productId, relatedImages));

        return result.Match(
            onValue: value => Results.Created($"/api/products/{productId}/related-images", value),
            onError: error => error.ToProblem()
        );
    }

    private static async Task<IResult> DeleteRelatedImage(
        [FromRoute] Guid productId,
        [FromBody] DeleteRelatedImageRequest request,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new DeleteRelatedImageFromProductCommand(productId, request.Url));

        return result.Match(
            onValue: value => Results.NoContent(),
            onError: error => error.ToProblem()
        );
    }

    private static async Task<IResult> EditRelatedImage(
        [FromRoute] Guid productId,
        [FromBody] EditRelatedImageRequest request,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new ReplaceRelatedImageCommand(productId, request.OldUrl, request.NewUrl, request.NewAltText));

        return result.Match(
            onValue: value => Results.NoContent(),
            onError: error => error.ToProblem()
        );
    }

    private static async Task<IResult> EditMainImage(
        [FromRoute] Guid productId,
        [FromBody] EditMainImageRequest request,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new ReplaceMainImageCommand(productId, request.NewMainUrl, request.NewAltText));

        return result.Match(
            onValue: value => Results.NoContent(),
            onError: error => error.ToProblem()
        );
    }

    #endregion

    #region  Product Discount Method

    private static async Task<IResult> AddDiscount(
        [FromRoute] Guid productId,
        [FromBody] AddDiscountRequest request,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new AddDiscountToProductCommand(productId, request.DiscountPercentage, request.DiscountEndDate));

        return result.Match(
            onValue: value => Results.Created($"/api/products/{productId}/discount", value),
            onError: error => error.ToProblem()
        );
    }

    private static async Task<IResult> DeleteDiscount(
        [FromRoute] Guid productId,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new DeleteDiscountFromProductCommand(productId));

        return result.Match(
                onValue: value => Results.NoContent(),
                onError: error => error.ToProblem()
            );
    }


    #endregion

    #region Product Features Method

    private static async Task<IResult> AddFeatures(
        [FromRoute] Guid productId,
        [FromBody] List<FeatureDto> features,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new AddFeaturesToProductCommand(productId, features));

        return result.Match(
            onValue: value => Results.Created($"/api/products/{productId}/features", value),
            onError: error => error.ToProblem()
        );
    }

    private static async Task<IResult> DeleteFeature(
        [FromRoute] Guid productId,
        [FromBody] DeleteFeatureRequest request,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new DeleteFeatureFromProductCommand(productId, request.Name));

        return result.Match(
            onValue: value => Results.NoContent(),
            onError: error => error.ToProblem()
        );
    }

    private static async Task<IResult> EditFeature(
        [FromRoute] Guid productId,
        [FromBody] EditFeatureRequest request,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new EditFeatureFromProductCommand(productId, request.OldName, request.NewName, request.NewValue));

        return result.Match(
            onValue: value => Results.NoContent(),
            onError: error => error.ToProblem()
        );
    }
             

    #endregion

    #region Main Product Method
    private static async Task<IResult> CreateProduct(
        [FromBody] CreateProductCommand command,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(command);

        return result.Match(
            onValue: value => Results.Created($"/api/products/{value}", value),
            onError: error => error.ToProblem()
        );
    }

    private static async Task<IResult> EditProduct(
        [FromRoute] Guid productId,
        [FromBody] EditProductRequest request,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new EditProductCommand(
           productId,
           request.Name,
           request.Description
        ));

        return result.Match(
            onValue: value => Results.NoContent(),
            onError: error => error.ToProblem()
        );
    }

    private static async Task<IResult> DeleteProduct(
        [FromRoute] Guid productId,
         [FromServices] ISender sender)
    {
        var result = await sender.Send(new DeleteProductCommand(productId));

        return result.Match(
            onValue: value => Results.NoContent(),
            onError: error => error.ToProblem()
        );
    } 

    private static async Task<IResult> GetProductById(
        [FromRoute] Guid productId,
        [FromServices] ISender sender)
   {
        var result = await sender.Send(new GetProductByIdQuery(productId));

        return result.Match(
            onValue: value => Results.Ok(value),
            onError: error => error.ToProblem()
        );
    }


    private static async Task<IResult> GetProductNamesByIds(
        [FromBody] GetProductNamesQuery query,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(query);

        return result.Match(
            onValue: value => Results.Ok(value),
            onError: error => error.ToProblem());
    }

    private static async Task<IResult> GetProducts(
        [AsParameters] GetProductsRequest request,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new GetProductsQuery(
            new PagingParams
            {
                Page = request.PageNumber,
                PageSize = request.PageSize,
                IsAscending = request.IsAscending,
                SortBy = request.SortBy,
            }
        ));

        return result.Match(
            onValue: value => Results.Ok(value),
            onError: error => error.ToProblem()
        );
    }

    private static async Task<IResult> EditProductStatus(
        [FromRoute] Guid productId,
        [FromBody] EditProductStatusRequest request,
        [FromServices] ISender sender)
    {
        var result = await sender.Send(new EditProductStatusCommand(productId, request.IsActive));

        return result.Match(
            onValue: value => Results.NoContent(),
            onError: error => error.ToProblem()
        );
    }

    #endregion

}
