namespace ProductService.Application.Features.Tags.Commands.EditTag;

public record EditTagCommand(Guid TagId, string Name) : ICommand<Unit>;