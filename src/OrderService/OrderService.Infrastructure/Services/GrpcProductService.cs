using Contracts.Product;
using Grpc.Core;
using OrderService.Application.Services;
using OrderService.Domain.DTOs;
using OrderService.Domain.Errors;
using SharedKernel.Primitives.Results;

namespace OrderService.Infrastructure.Services;

public class GrpcProductService : IProductService
{
    private readonly ProductProtoService.ProductProtoServiceClient _productClient;

    public GrpcProductService(ProductProtoService.ProductProtoServiceClient productClient)
    {
        _productClient = productClient;
    }

    public async Task<Result<ProductDto>> GetProductByIdAsync(Guid id)
    {
        try
        {
            var product = await _productClient.GetProductByIdAsync(new GetProudctByIdRequest { Id = id.ToString() });
            if (product is null) return DomainErrors.Product.NotFound(id);

            return ProductDto.FromGrpcModel(product);
        }
        catch(RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            return DomainErrors.Product.NotFound(id);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.DeadlineExceeded)
        {
            return DomainErrors.System.RemoteTimeout;
        }
        catch (RpcException)
        {
            return DomainErrors.System.InternalError;
        }

    }

    public async Task<Result<List<ProductDto>>> GetProductsByIdsAsync(List<Guid> ids)
    {
        var modelIds = new GetProductsByIdsRequest();
        modelIds.Ids.AddRange(ids.Select(i => i.ToString()));

        var products = await _productClient.GetProductsByIdsAsync(modelIds);

        return products.Products.Select(p => ProductDto.FromGrpcModel(p)).ToList();
    }
}
