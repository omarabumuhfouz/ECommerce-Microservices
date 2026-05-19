namespace ProductService.Application.Features.Categories.Commands.RestoreCategory;

public sealed class RestoreCategoryCommandHandler : ICommandHandler<RestoreCategoryCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RestoreCategoryCommandHandler> _logger;

    public RestoreCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<RestoreCategoryCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(RestoreCategoryCommand request, CancellationToken ct)

    {
        // 1. Pre-Log Intent
        _logger.LogInformation("Starting restoration process for Category with Id: {CategoryId}", request.CategoryId);

        var repository = _unitOfWork.GetRepository<Category>();

        return await repository
            // 2. Fetch (Crucial: Uses the spec that ignores global query filters)
            .FirstOrDefaultAsync(new GetCategoryByIdWithDeletedSpec(request.CategoryId), ct)

            // 3. Null Check -> Convert to Failure if missing
            .ToResult(DomainErrors.Category.NotFound(request.CategoryId))

            // 4. Execute Domain Logic
            // category.Restore() returns Result. We map it back to the 'category' object 
            // so the success logger down the track has access to its Name.
            .Bind(category => category.Restore().Map(() => category))

            // 5. Persistence
            .Tap(category => repository.Update(category))
            .Tap(async _ => await _unitOfWork.SaveChangesAsync(ct))

            // 6. Success Logging
            .Tap(category => _logger.LogInformation(
                "Category '{CategoryName}' with Id: {CategoryId} was restored successfully.",
                category.Name, category.Id))

            // 7. Centralized Error Logging
            // Catches NotFound, or if Restore() fails for any domain reason
            .TapError(error => _logger.LogError(
                "Failed to restore Category with Id: {CategoryId}. Error Code: {ErrorCode}, Message: {ErrorMessage}",
                request.CategoryId, error.Code, error.Message))

            // 8. Map to MediatR's expected Unit type
            .Map(_ => Unit.Value);
    }
}