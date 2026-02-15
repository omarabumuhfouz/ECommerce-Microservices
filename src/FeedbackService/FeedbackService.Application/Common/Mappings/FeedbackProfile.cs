namespace FeedbackService.Application.Common.Mappings;

public class FeedbackProfile : Profile
{
    public FeedbackProfile()
    {
        CreateMap<Feedback, FeedbackDto>()
                    // Map Value Object 'Rating' to int
                    .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating.Value))

                    // Map Value Object 'Comment' to string
                    // We use the null-conditional operator in case Comment is nullable
                    .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment.Value));

    }
}