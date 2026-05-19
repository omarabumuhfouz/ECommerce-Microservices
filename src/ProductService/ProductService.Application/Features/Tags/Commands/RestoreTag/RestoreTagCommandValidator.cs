namespace ProductService.Application.Features.Tags.Commands.RestoreTag;

public sealed class RestoreTagCommandValidator : AbstractValidator<RestoreTagCommand>
{
    public RestoreTagCommandValidator()
    {
        RuleFor(t => t.TagId)
        .ValidateTagId();
    }
}