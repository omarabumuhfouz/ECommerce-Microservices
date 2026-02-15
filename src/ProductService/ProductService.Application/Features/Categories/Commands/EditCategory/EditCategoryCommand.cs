namespace ProductService.Application.Features.Categories.Commands.EditCategory;
public record EditCategoryCommand(Guid CategoryId, string Name, string Description) : ICommand<Unit>;
