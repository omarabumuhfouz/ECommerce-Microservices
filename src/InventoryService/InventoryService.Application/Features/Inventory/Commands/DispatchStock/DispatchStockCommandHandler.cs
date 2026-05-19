using SharedKernel.Common;

namespace InventoryService.Application.Features.Inventory.Commands.DispatchStock;

public class DispatchStockCommandHandler : ICommandHandler<DispatchStockCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DispatchStockCommandHandler> _logger;

    public DispatchStockCommandHandler(IUnitOfWork unitOfWork, ILogger<DispatchStockCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(DispatchStockCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Starting Dispatch Stock for Order: {@OrderId}, Product: {@ProductId}",
                request.OrderId, request.ProductId);

        return await _unitOfWork.GetRepository<InventoryItem>()

        .FirstOrDefaultAsync(new InventoryItemByProductIdSpec(request.ProductId), ct)
             .ToResult(DomainErrors.Inventory.NotFoundByProductId(request.ProductId))

        .Bind(inventoryItem => inventoryItem.DispatchStock(request.OrderId))
        .Tap(async () => await _unitOfWork.SaveChangesAsync(ct))

        .Tap(() => _logger.LogInformation("Dispatch Stock Succcessfully for Order : {@OrderId}, Related to product :{@ProductId}",
             request.OrderId,
            request.ProductId))

        .TapError(error => _logger.LogError(
            "Failed to Dispatch Stock for Order {@OrderId}. Product: {@ProductId}. Error Code: {ErrorCode}, Message: {ErrorMessage}", 
            request.OrderId, request.ProductId, error.Code, error.Message))

        .Map(_ => Unit.Value);
    }
}