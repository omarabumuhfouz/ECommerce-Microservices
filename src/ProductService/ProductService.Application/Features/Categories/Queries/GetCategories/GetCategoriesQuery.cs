using ProductService.Application.Features.Categories.DTOs;

namespace ProductService.Application.Features.Categories.Queries.GetCategories;

public record GetCategoriesQuery : IQuery<List<CategoryDto>>;
