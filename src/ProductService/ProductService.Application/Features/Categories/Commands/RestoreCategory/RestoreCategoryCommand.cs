namespace ProductService.Application.Features.Categories.Commands.RestoreCategory;

public sealed record RestoreCategoryCommand(Guid CategoryId) : ICommand<Unit>;