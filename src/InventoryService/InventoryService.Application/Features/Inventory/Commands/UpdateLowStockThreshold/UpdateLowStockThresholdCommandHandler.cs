using SharedKernel.Common;

namespace InventoryService.Application.Features.Inventory.Commands.UpdateLowStockThreshold;

public class UpdateLowStockThresholdCommandHandler : ICommandHandler<UpdateLowStockThresholdCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateLowStockThresholdCommandHandler> _logger;

    public UpdateLowStockThresholdCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateLowStockThresholdCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(UpdateLowStockThresholdCommand request, CancellationToken cancellationToken)
{
    // 1. Pre-Log: Trace the intent (Useful for audit trails on configuration changes)
    _logger.LogInformation(
        "Updating Low Stock Threshold for Product {ProductId} to {NewThreshold}", 
        request.ProductId, request.NewThreshold);

    return await _unitOfWork.GetRepository<InventoryItem>()

        // 2. Fetch the Aggregate Root
        .FirstOrDefaultAsync(new InventoryItemByProductIdSpec(request.ProductId), cancellationToken)

        // 3. Null Check -> Result
        // Converts a null Entity into a standardized "Not Found" Failure
        .ToResult(DomainErrors.Inventory.NotFoundByProductId(request.ProductId))

        // 4. Domain Logic execution
        // We use .Bind() because UpdateLowStockThreshold returns a Result (it might fail if Threshold < 0)
        // This switches the track from Result<InventoryItem> to Result (Void)
        .Bind(inventoryItem => inventoryItem.UpdateLowStockThreshold(request.NewThreshold))

        // 5. Persistence
        // Only executes if both the Item was found AND the Threshold was valid
        .Tap(async () => await _unitOfWork.SaveChangesAsync(cancellationToken))

        // 6. Success Logging
        .Tap(() => _logger.LogInformation(
            "Successfully updated Low Stock Threshold for Product {ProductId} to {NewThreshold}",
            request.ProductId, request.NewThreshold))

        // 7. Error Logging (Centralized)
        // Catches both "Item Not Found" and "Invalid Threshold" errors
        .TapError(error => _logger.LogError(
            "Failed to update Low Stock Threshold for Product {ProductId}. Error Code: {ErrorCode}, Message: {ErrorMessage}", 
            request.ProductId, error.Code, error.Message))

        // 8. Return Unit (Standard MediatR pattern)
        .Map(_ => Unit.Value);
}
}