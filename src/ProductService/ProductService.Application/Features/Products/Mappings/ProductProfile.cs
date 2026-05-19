using ProductService.Application.Features.Products.DTOs;
using ProductService.Domain.Products;

namespace ProductService.Application.Mappings;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Price.Currency))
            .ForMember(dest => dest.MainImage, opt => opt.MapFrom(src => new ImageDto(src.MainImage.Url, src.MainImage.AltText)))
            .ForMember(dest => dest.DiscountPercentage, opt => opt.MapFrom(src => src.Discount != null ? src.Discount.Percentage : 0));


        CreateMap<Product, ProductDetailsDto>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Price.Currency))
            .ForMember(dest => dest.MainImage, opt => opt.MapFrom(src => new ImageDto(src.MainImage.Url, src.MainImage.AltText)))
            .ForMember(dest => dest.RelatedImages, opt => opt.MapFrom(src => src.RelatedImages.Select(ri => new ImageDto(ri.Url, ri.AltText)).ToList()))
            .ForMember(dest => dest.DiscountPercentage, opt => opt.MapFrom(src => src.Discount != null ? src.Discount.Percentage : 0))
            .ForMember(dest => dest.DiscountEndDate, opt => opt.MapFrom(src => src.Discount != null ? src.Discount.EndDate : (DateTime?)null))
            .ForMember(dest => dest.Features, opt => opt.MapFrom(src => src.Features.Select(f => new FeatureDto(f.Name, f.Value)).ToList()))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.Name).ToList()));
    }
}