namespace ProductService.Application.Features.Tags.Commands.AddTag;
public class AddTagCommandValidator : AbstractValidator<AddTagCommand>
{
    public AddTagCommandValidator()
    {
        RuleFor(t => t.Name)
        .ValidateTagName();
    }
}