namespace ProductService.Application.Features.Tags.Commands.DeleteTag;

public record DeleteTagCommand(Guid TagId) : ICommand<Unit>;