using SharedKernel.Common;

namespace InventoryService.Application.Features.Inventory.Commands.ReceiveStock;

public class ReceiveStockCommandHandler : ICommandHandler<ReceiveStockCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ReceiveStockCommandHandler> _logger;

    public ReceiveStockCommandHandler(IUnitOfWork unitOfWork, ILogger<ReceiveStockCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(ReceiveStockCommand request, CancellationToken ct)
    {
        _logger.LogInformation(
                "Starting Receive Stock for Product {ProductId}. Amount: {Amount}",
                request.ProductId, request.Amount);

        return await _unitOfWork.GetRepository<InventoryItem>()

        .FirstOrDefaultAsync(new InventoryItemByProductIdSpec(request.ProductId), ct)

        .ToResult(DomainErrors.Inventory.NotFoundByProductId(request.ProductId))

        .Bind(inventoryItem => inventoryItem.ReceiveStock(request.Amount))

        .Tap(async () => await _unitOfWork.SaveChangesAsync(ct))

        .Tap(() => _logger.LogInformation(
            "Successfully Received Stock for Product {ProductId}. Amount Added: {Amount}",
            request.ProductId, request.Amount))

        .TapError(error => _logger.LogError(
                "Failed to Receive Stock for Product {ProductId}. Error Code: {ErrorCode}, Message: {ErrorMessage}",
                request.ProductId, error.Code, error.Message))
        .Map(_ => Unit.Value);

    }
}