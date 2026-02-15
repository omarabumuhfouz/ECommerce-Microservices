namespace ProductService.Application.Features.Categories.Commands.CreateCategory;
public record CreateCategoryCommand(string Name, string Description) : ICommand<Guid>;
