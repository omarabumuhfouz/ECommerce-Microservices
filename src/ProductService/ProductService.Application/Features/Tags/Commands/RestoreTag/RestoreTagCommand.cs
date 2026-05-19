namespace ProductService.Application.Features.Tags.Commands.RestoreTag;

public sealed record RestoreTagCommand(Guid TagId) : ICommand<Unit>;