namespace ProductService.Application.Features.Categories.Commands.SetCategoryAsActive;
public record SetCategoryAsActiveCommand(Guid CategoryId) : ICommand<Unit>;
