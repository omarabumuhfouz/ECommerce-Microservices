using SharedKernel.Common;

namespace InventoryService.Application.Features.Inventory.Commands.ReserveStock;

public class ReserveStockCommandHandler : ICommandHandler<ReserveStockCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ReserveStockCommandHandler> _logger;

    public ReserveStockCommandHandler(IUnitOfWork unitOfWork, ILogger<ReserveStockCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(ReserveStockCommand request, CancellationToken cancellationToken)
    {
        // 1. Pre-Log: Trace the intent (Vital for debugging high-traffic reservation issues)
        _logger.LogInformation(
            "Starting Stock Reservation for Order {OrderId}. Product: {ProductId}, Amount: {Amount}",
            request.OrderId, request.ProductId, request.Amount);

        return await _unitOfWork.GetRepository<InventoryItem>()
            // 2. Fetch Async
            .FirstOrDefaultAsync(new InventoryItemByProductIdSpec(request.ProductId), cancellationToken)

            // 3. Null Check -> Result (Standardizes 'null' to a Domain Error)
            .ToResult(DomainErrors.Inventory.NotFoundByProductId(request.ProductId))

            // 4. Domain Logic (Bind checks if ReserveStock succeeds or fails)
            // If this returns Failure (Insufficient Stock), the chain skips to Step 7
            .Bind(inventoryItem => inventoryItem.ReserveStock(request.OrderId, request.Amount))

            // 5. Persistence (Only runs if ReserveStock succeeded)
            .Tap(async () => await _unitOfWork.SaveChangesAsync(cancellationToken))

            // 6. Success Logging
            .Tap(() => _logger.LogInformation(
                "Successfully Reserved Stock for Order {OrderId}. Product: {ProductId}, Amount: {Amount}",
                request.OrderId, request.ProductId, request.Amount))

            // 7. Error Logging (Catches NotFound, InsufficientStock, or NegativeAmount)
            .TapError(error => _logger.LogError(
                "Failed to Reserve Stock for Order {OrderId}. Product: {ProductId}. Error Code: {ErrorCode}, Message: {ErrorMessage}",
                request.OrderId, request.ProductId, error.Code, error.Message))

            // 8. Map to Unit for MediatR
            .Map(_ => Unit.Value);
    }
}