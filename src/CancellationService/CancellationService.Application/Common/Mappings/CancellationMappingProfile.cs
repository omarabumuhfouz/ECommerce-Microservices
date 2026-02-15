using AutoMapper;

namespace CancellationService.Application.Common.Mappings;

public class CancellationMappingProfile : Profile
{
    public CancellationMappingProfile()
    {
        CreateMap<Cancellation, CancellationDto>()
            // 1. Map Simple Properties
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString())) // Enum to String
            .ForMember(dest => dest.RequestedAt, opt => opt.MapFrom(src => src.RequestedAt))
            .ForMember(dest => dest.ProcessedAt, opt => opt.MapFrom(src => src.ProcessedAt))
            .ForMember(dest => dest.ProcessedBy, opt => opt.MapFrom(src => src.ProcessedBy))

            // 2. Map Value Objects (Extract .Value)
            .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.Reason.Value))
            
            .ForMember(dest => dest.OrderAmount, opt => opt.MapFrom(src => src.OrderAmount.Value))
            
            .ForMember(dest => dest.CancellationCharges, opt => opt.MapFrom(src => src.Charges.Value))
            
            // 3. Map Nullable Value Object
            .ForMember(dest => dest.Remarks, opt => opt.MapFrom(src => src.Remarks != null ? src.Remarks.Value : null));
    }
}
