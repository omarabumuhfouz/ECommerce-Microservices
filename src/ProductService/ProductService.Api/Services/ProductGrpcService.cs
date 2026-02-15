using Contracts.Product;
using Grpc.Core;
using MediatR;
using ProductService.Application.Enums;
using ProductService.Application.Features.Products.Commands.EditStock;
using ProductService.Application.Features.Products.Queries.GetProductById;
using ProductService.Application.Features.Products.Queries.GetProductNames;
using ProductService.Application.Features.Products.Queries.GetProductsByIds;
using ProductService.Application.Features.Products.Specifications;
using SharedKernel.Extensions;

namespace ProductService.Api.Services;

public class ProductGrpcService(ISender sender) : ProductProtoService.ProductProtoServiceBase
{
    public override async Task<ModelResponse> DecreaseStock(DecreaseStockRequest request, ServerCallContext context)
    {
        if(!Guid.TryParse(request.ProductId, out var productId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid ProductId format."));
        }

        var result = (await sender.Send(new EditStockCommand(productId, request.Quantity, StockOperation.Increase)))
                                .ValueOrThrowRpc();

        return new ModelResponse
        {
            Message = "Decrease Stock operation completed successfully.",
            Success = true,
        };
    }

    public override async  Task<GetProducsNamesByIdsResponse> GetProducsNamesByIds(GetProducsNamesByIdsRequest request, ServerCallContext context)
    {
        var validGuids = new List<Guid>();

        foreach (var idString in request.ProductIds)
        {
            if (!Guid.TryParse(idString, out var guidId))
            {
                throw new RpcException(new Status(
                    StatusCode.InvalidArgument,
                    $"The product ID '{idString}' is not a valid GUID format."));
            }
            validGuids.Add(guidId);
        }

        var productNamesResult = (await sender.Send(new GetProductNamesQuery(validGuids)))
                             .ValueOrThrowRpc();

        var response = new GetProducsNamesByIdsResponse();

        response.ProductsNames.AddRange(productNamesResult.Select(pn => new ProductNameModel
        {
            ProductId = pn.Id.ToString(),
            Name = pn.Name
        }));

        return response;
    }

    public override async Task<ProductModel> GetProductById(GetProudctByIdRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.Id, out var productId))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid ProductId format."));
        }

        var product = (await sender.Send(new GetProductByIdQuery(productId)))
                                .ValueOrThrowRpc();


        return product.ToGrpcModel();
    }

    public override async Task<GetProductsByIdsResponse> GetProductsByIds(GetProductsByIdsRequest request, ServerCallContext context)
    {
        var validGuids = new List<Guid>();

        foreach (var idString in request.Ids)
        {
            if (!Guid.TryParse(idString, out var guidId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, $"Invalid ProductId format: {idString}"));
            }
            validGuids.Add(guidId);
        }

        var products = (await sender.Send(new GetProductsByIdsQuery(validGuids))).ValueOrThrowRpc();

        var response = new GetProductsByIdsResponse();
        response.Products.AddRange(products.Select(p => p.ToGrpcModel()));

        return response;
    }
}