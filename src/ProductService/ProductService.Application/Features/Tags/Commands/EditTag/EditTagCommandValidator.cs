namespace ProductService.Application.Features.Tags.Commands.EditTag;

public class EditTagCommandValidator : AbstractValidator<EditTagCommand>
{
    public EditTagCommandValidator()
    {
        RuleFor(c => c.TagId)
            .ValidateTagId();

        RuleFor(c => c.Name)
            .ValidateTagName();
    }
    
}