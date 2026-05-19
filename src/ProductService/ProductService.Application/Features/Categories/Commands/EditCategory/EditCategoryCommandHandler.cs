namespace ProductService.Application.Features.Categories.Commands.EditCategory;

public class EditCategoryCommandHandler : ICommandHandler<EditCategoryCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EditCategoryCommandHandler> _logger;

    public EditCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<EditCategoryCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async Task<Result<Unit>> IRequestHandler<EditCategoryCommand, Result<Unit>>.Handle(EditCategoryCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Starting update process for Category with Id: {CategoryId}", request.CategoryId);

        var categoryRepo = _unitOfWork.GetRepository<Category>();
        var category = await categoryRepo.FirstOrDefaultAsync(new GetCategoryByIdSpec(request.CategoryId), ct);

        if (category == null) return DomainErrors.Category.NotFound(request.CategoryId);

        if (await categoryRepo.AnyAsync(c => c.Name.Value == request.Name && c.Id != request.CategoryId, ct))
                return DomainErrors.Category.DuplicateName(request.Name);

        return category.Edit(request.Name, request.Description)

        .Tap(_ => categoryRepo.Update(category))

        .Tap(async _ => await _unitOfWork.SaveChangesAsync(ct))

        .Tap(_ => _logger.LogInformation("Category with id: {CategoryId} has been updated.", request.CategoryId))

        .TapError(error => _logger.LogError("Failed to update Category logic. Error: {ErrorMessage}", error.Message))

        .Map(_ => Unit.Value);
    }
}
