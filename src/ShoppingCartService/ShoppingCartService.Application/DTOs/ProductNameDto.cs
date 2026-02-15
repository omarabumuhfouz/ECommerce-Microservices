using Contracts.Product;

namespace ShoppingCartService.Application.DTOs;

public record ProductNameDto(Guid Id, string Name)
{
    public static List<ProductNameDto> FromGrpcModel(GetProducsNamesByIdsResponse response)
    {
        return response.ProductsNames.Select(p =>
                new ProductNameDto(Guid.Parse(p.ProductId), p.Name)).ToList();
    }    
}
