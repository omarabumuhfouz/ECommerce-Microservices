namespace ProductService.Application.Features.Tags.Commands.DeleteTag;

public class DeleteTagCommandValidator : AbstractValidator<DeleteTagCommand>
{
    public DeleteTagCommandValidator()
    {
        RuleFor(t => t.TagId)
            .ValidateTagId();
    }
}