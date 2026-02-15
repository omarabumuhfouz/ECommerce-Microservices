namespace ProductService.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteCategoryCommandHandler> _logger;

    public DeleteCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteCategoryCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    async Task<Result<Unit>> IRequestHandler<DeleteCategoryCommand, Result<Unit>>.Handle(DeleteCategoryCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Starting deletion process for Category with Id: {CategoryId}", request.CategoryId);

        var categoryRepository = _unitOfWork.GetRepository<Category>();
        var category = await categoryRepository.GetSingleBySpecAsync(new GetCategoryByIdSpec(request.CategoryId), ct);

        if (category is null) return DomainErrors.Category.NotFound(request.CategoryId);

        if (category.IsActive is false) return Unit.Value;

        // Check if category has associated products
        
        if (await _unitOfWork.GetRepository<Product>().IsExistsAsync(p => p.CategoryId == request.CategoryId, ct))
            return DomainErrors.Category.HasProducts(request.CategoryId);

         category.EditStatus(false);

        categoryRepository.Update(category);
        await _unitOfWork.SaveChangesAsync(ct);

        _logger.LogInformation(
                    "Category '{CategoryName}' with Id: {CategoryId} was deactivated successfully.",
                    category.Name,
                    category.Id);

        return Unit.Value;
    }
}
