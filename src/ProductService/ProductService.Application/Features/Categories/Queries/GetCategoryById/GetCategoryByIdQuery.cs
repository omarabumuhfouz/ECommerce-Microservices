using ProductService.Application.Features.Categories.DTOs;

namespace ProductService.Application.Features.Categories.Queries.GetCategoryById;

public record GetCategoryByIdQuery(Guid CategoryId) : IQuery<CategoryDto>;
