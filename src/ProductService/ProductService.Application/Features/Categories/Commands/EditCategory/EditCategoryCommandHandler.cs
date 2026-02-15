
namespace ProductService.Application.Features.Categories.Commands.EditCategory;

public class EditCategoryCommandHandler : ICommandHandler<EditCategoryCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EditCategoryCommandHandler> _logger;

    public EditCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<EditCategoryCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async Task<Result<Unit>> IRequestHandler<EditCategoryCommand, Result<Unit>>.Handle(EditCategoryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting update process for Category with Id: {CategoryId}", request.CategoryId);

        var categoryRepo = _unitOfWork.GetRepository<Category>();
        var category = await categoryRepo.GetSingleBySpecAsync(new GetCategoryByIdSpec(request.CategoryId), cancellationToken);

        if (category == null) return DomainErrors.Category.NotFound(request.CategoryId);

        // Check if a category with the same name already exists (excluding the current one)
        if (await categoryRepo.IsExistsAsync(c => c.Name.Value == request.Name && c.Id != request.CategoryId, cancellationToken))
                return DomainErrors.Category.DuplicateName(request.Name);

         var updatingResult = category.Edit(request.Name, request.Description);
        if (updatingResult.IsFailure) return updatingResult.TopError;

        categoryRepo.Update(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Category with id: {CategoryId} has been updated.", request.CategoryId);

        return Unit.Value;
    }
}
