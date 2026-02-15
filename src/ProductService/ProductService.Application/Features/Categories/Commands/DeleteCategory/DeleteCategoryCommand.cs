namespace ProductService.Application.Features.Categories.Commands.DeleteCategory;
public record DeleteCategoryCommand(Guid CategoryId) : ICommand<Unit>;

