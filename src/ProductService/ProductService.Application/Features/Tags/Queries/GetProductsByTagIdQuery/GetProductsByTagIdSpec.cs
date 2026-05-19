using ProductService.Application.Features.Products.DTOs;

namespace ProductService.Application.Features.Products.Queries.GetProductsByTagId;

public sealed class GetProductsByTagIdSpec : Specification<Product, ProductDto>
{
    public GetProductsByTagIdSpec(Guid tagId)
    {
        Query.AsNoTracking()
             // Finds any product where the Tags list contains a tag matching the given ID
             .Where(product => product.Tags.Any(pt => pt.Id == tagId)) 
             .Select(product => new ProductDto
             {
                 Id = product.Id,
                 Name = product.Name, 
                 Description = product.Description, 
                 Price = product.Price.Amount, 
                 Currency = product.Price.Currency,
                 DiscountPercentage = product.Discount.Percentage,
                 MainImage = new ImageDto(product.MainImage.Url, product.MainImage.AltText)
             });
    }
}