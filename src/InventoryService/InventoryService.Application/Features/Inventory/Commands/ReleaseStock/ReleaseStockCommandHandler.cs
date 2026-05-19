using SharedKernel.Common;

namespace InventoryService.Application.Features.Inventory.Commands.ReleaseStock;

public class ReleaseStockCommandHandler : ICommandHandler<ReleaseStockCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ReleaseStockCommandHandler> _logger;

    public ReleaseStockCommandHandler(IUnitOfWork unitOfWork, ILogger<ReleaseStockCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(ReleaseStockCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Release Stock for Order {OrderId}. Product: {ProductId}",
            request.OrderId, request.ProductId);

        return await _unitOfWork.GetRepository<InventoryItem>()

            .FirstOrDefaultAsync(new InventoryItemByProductIdSpec(request.ProductId), cancellationToken)

            .ToResult(DomainErrors.Inventory.NotFoundByProductId(request.ProductId))

            .Bind(inventoryItem => inventoryItem.ReleaseReservedStock(request.OrderId))

            .Tap(async () => await _unitOfWork.SaveChangesAsync(cancellationToken))

            .Tap(() => _logger.LogInformation(
                "Successfully Released Stock for Order {OrderId}. Product: {ProductId}",
                request.OrderId, request.ProductId))

            .TapError(error => _logger.LogError(
                "Failed to Release Stock for Order {OrderId}. Product: {ProductId}. Error: {Error}",
                request.OrderId, request.ProductId, error))

            .Map(_ => Unit.Value);
    }
}