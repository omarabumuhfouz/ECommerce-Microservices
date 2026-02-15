using AutoMapper;

namespace PaymentService.Application.Mappings
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Payment, PaymentDto>()
                .ForMember(dest => dest.PaymentDate, src => src.MapFrom(p => p.CreatedOnUtc));
        }
    }
}
