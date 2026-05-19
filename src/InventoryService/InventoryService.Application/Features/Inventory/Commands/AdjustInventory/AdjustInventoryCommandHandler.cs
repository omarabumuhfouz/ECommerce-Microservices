using SharedKernel.Common;

namespace InventoryService.Application.Features.Inventory.Commands.AdjustInventory;

public sealed class AdjustInventoryCommandHandler 
    : ICommandHandler<AdjustInventoryCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public AdjustInventoryCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(AdjustInventoryCommand request, CancellationToken ct)
    {
        _logger.LogInformation(
                "Starting Inventory Adjustment for Product {ProductId}. Order: {OrderId}, Delta: {QuantityDelta}",
                request.ProductId, request.OrderId, request.QuantityDelta);

        return await _unitOfWork.GetRepository<InventoryItem>()

        .FirstOrDefaultAsync(new InventoryItemByProductIdSpec(request.ProductId), ct)
        .ToResult(DomainErrors.Inventory.NotFoundByProductId(request.ProductId))
        .TapError(error =>  _logger.LogError(""))

        .Bind(inventoryItem => inventoryItem.AdjustReservation(request.OrderId, request.QuantityDelta))

        .Tap(async () => await _unitOfWork.SaveChangesAsync(ct))

        .Tap(() => _logger.LogInformation(
            "Successfully adjusted inventory for Product {ProductId}. Order: {OrderId}, Delta: {QuantityDelta}",
            request.ProductId, request.OrderId, request.QuantityDelta))
        
        .TapError(error => _logger.LogError(
                "Failed to adjust inventory for Product {ProductId}. Error Code: {ErrorCode}, Message: {ErrorMessage}", 
                request.ProductId, error.Code, error.Message))

        .Map(_ => Unit.Value);

    }
}