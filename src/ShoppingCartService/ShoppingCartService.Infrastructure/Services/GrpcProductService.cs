using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Contracts.Product;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using SharedKernel.Shared;
using ShoppingCartService.Application.DTOs;
using ShoppingCartService.Application.Services;
using ShoppingCartService.Domain.Constants;
using ShoppingCartService.Domain.Errors;

namespace ShoppingCartService.Infrastructure.Services;

public class GrpcProductService : IProductService
{
    private readonly ProductProtoService.ProductProtoServiceClient _grpcClients;

    public GrpcProductService(ProductProtoService.ProductProtoServiceClient grpcClients)
    {
        _grpcClients = grpcClients;
    }

    public async Task<Result<ProductDto>> GetProductByIdAsync(Guid id)
    {
        try
        {
            return ProductDto.FromGrpcModel(await _grpcClients.GetProductByIdAsync(
                        new GetProudctByIdRequest { Id = id.ToString() }));

        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {

            return DomainErrors.Product.NotFound(id);
        }
        catch(RpcException ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }


    public async Task<Result<List<ProductNameDto>>> GetProductNamesByIdsAsync(List<Guid> productIds)
    {
        try
        {
            if (productIds is null || !productIds.Any()) return new List<ProductNameDto>();

            var request = new GetProducsNamesByIdsRequest();

            request.ProductIds.AddRange(productIds.Select(p => p.ToString()));


            return ProductNameDto.FromGrpcModel(await _grpcClients.GetProducsNamesByIdsAsync(request));
        }
        catch(RpcException ex) when (ex.StatusCode == StatusCode.InvalidArgument)
        {
            throw new InvalidOperationException(ex.Message);
        }
        catch(RpcException ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }

}
