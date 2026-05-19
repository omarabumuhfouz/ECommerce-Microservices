using ProductService.Application.Features.Products.DTOs;
using ProductService.Domain.Products;

namespace ProductService.Application.Features.Products.Specifications;
public class GetProductsNamesSpec : Specification<Product, ProductNameDto>
{
    public GetProductsNamesSpec(List<Guid> ProductIds) : base(p => ProductIds.Contains(p.Id))
    {
        AddSelect(p => new ProductNameDto(p.Id, p.Name));
    }
}