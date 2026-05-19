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

    public async Task<Result<Unit>> Handle(DeleteCategoryCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Starting deletion process for Category with Id: {CategoryId}", request.CategoryId);

        var categoryRepository = _unitOfWork.GetRepository<Category>();
        var productRepository = _unitOfWork.GetRepository<Product>();

        return await categoryRepository
            // 1. Fetch
            .FirstOrDefaultAsync(new GetCategoryByIdSpec(request.CategoryId), ct)

            // 2. Null Check -> Result
            .ToResult(DomainErrors.Category.NotFound(request.CategoryId))

            // 3. Async Business Rules & Checks (Idempotency & Product Association)
            .Bind(async category =>
            {
                // Idempotency: If already deleted, skip the rest and return success
                if (category.IsDeleted) return Result.Success(category);

                // Check if products are associated
                bool hasProducts = await productRepository.AnyAsync(p => p.CategoryId == request.CategoryId, ct);
                if (hasProducts) return Result.Failure<Category>(DomainErrors.Category.HasProducts(request.CategoryId));

                return Result.Success(category);
            })

            // 4. Execute Domain Logic
            // category.Delete() returns a Result. We map it back to the 'category' object 
            // so we can pass it down the railway for the success logger to read its Name.
            .Bind(category => category.Delete().Map(() => category))

            // 5. Persistence 
            .Tap(category => categoryRepository.Update(category))
            .Tap(async _ => await _unitOfWork.SaveChangesAsync(ct))

            // 6. Success Logging (Only hits if everything above succeeded)
            .Tap(category => _logger.LogInformation(
                "Category '{CategoryName}' with Id: {CategoryId} was deactivated successfully.",
                category.Name, category.Id))

            // 7. Error Logging (Catches NotFound, HasProducts, or Domain Logic failures)
            .TapError(error => _logger.LogError(
                "Failed to delete Category with Id: {CategoryId}. Error Code: {ErrorCode}, Message: {ErrorMessage}",
                request.CategoryId, error.Code, error.Message))

            // 8. Map to MediatR's expected return type
            .Map(_ => Unit.Value);
    }
}
