namespace ProductService.Application.Features.Tags.Commands.AddTag;

public record AddTagCommand(string Name) : ICommand<Guid>;