namespace ProductService.Application.Features.Products.Commands.RestoreProduct;

public sealed class RestoreProductCommandHandler : IRequestHandler<RestoreProductCommand, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RestoreProductCommandHandler> _logger;

    public RestoreProductCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<RestoreProductCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(RestoreProductCommand request, CancellationToken ct)
    {
        // 1. Pre-Log Intent
        _logger.LogInformation("Starting restoration process for Product with Id: {ProductId}", request.ProductId);

        var repository = _unitOfWork.GetRepository<Product>();

        return await repository
            // 2. Fetch (Crucial: Uses the spec that ignores global query filters)
            .FirstOrDefaultAsync(new GetProductByIdWithDeletedSpec(request.ProductId), ct)

            // 3. Null Check -> Convert to Failure if missing
            .ToResult(DomainErrors.Product.NotFound(request.ProductId))

            // 4. Execute Domain Logic
            // product.Restore() returns Result. We map it back to the 'product' object 
            // so the success logger down the track has access to its Name.
            .Bind(product => product.Restore().Map(() => product))

            // 5. Persistence
            .Tap(product => repository.Update(product))
            .Tap(async product => { await _unitOfWork.SaveChangesAsync(ct); }) // Enclosed in {} to ignore the int result

            // 6. Success Logging
            .Tap(product => _logger.LogInformation(
                "Product '{ProductName}' with Id: {ProductId} was restored successfully.",
                product.Name, product.Id))

            // 7. Centralized Error Logging
            // Catches NotFound, or if Restore() fails for any domain reason
            .TapError(error => _logger.LogError(
                "Failed to restore Product with Id: {ProductId}. Error Code: {ErrorCode}, Message: {ErrorMessage}",
                request.ProductId, error.Code, error.Message))

            // 8. Map to MediatR's expected Unit type
            .Map(_ => Unit.Value);
    }
}