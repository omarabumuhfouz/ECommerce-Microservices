using ProductService.Application.Features.Categories.DTOs;
using ProductService.Domain.Categories;

namespace ProductService.Application.Mappings;
public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDto>()
                .ConstructUsing(c => new CategoryDto(c.Id, c.Name.Value, c.Description, c.IsActive))
                     .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value));
        // CreateMap<CategorySeedDto, Category>()
        //     .ConstructUsing((dto, context) =>
        //         new Category(
        //             dto.Id,
        //             dto.Name,
        //             dto.Description,
        //             dto.IsActive,
        //             context.Mapper.Map<ICollection<Product>>(dto.Products)
        //         ));
    }
}
