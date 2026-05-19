namespace InventoryService.Application.Features.Inventory.Commands.Create;

public class CreateInventoryCommandHandler : ICommandHandler<CreateInventoryCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateInventoryCommandHandler> _logger;

    public CreateInventoryCommandHandler(
        IUnitOfWork unitOfWork, 
        ILogger<CreateInventoryCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateInventoryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating Inventory record for Product: {ProductId}", request.ProductId);

        var repository = _unitOfWork.GetRepository<InventoryItem>();

        // 1. Check if an inventory record already exists for this product
        bool exists = await repository.AnyAsync(x => x.ProductId == request.ProductId, cancellationToken);
        
        if (exists) 
        {
            _logger.LogWarning("Inventory already exists for Product {ProductId}", request.ProductId);
            return DomainErrors.Inventory.AlreadyExists(request.ProductId); // You'll need to add this error
        }

        // 2. Execute Domain Logic & ROP Pipeline
        return InventoryItem.Create(request.ProductId, request.InitialStock, request.LowStockThreshold)

            .Tap(async item => await repository.AddAsync(item))
            .Tap(async () => await _unitOfWork.SaveChangesAsync(cancellationToken))

            .Tap(item => _logger.LogInformation(
                "Successfully created Inventory {InventoryId} for Product {ProductId}",
                item.Id, request.ProductId))

            .TapError(error => _logger.LogError(
                 "Failed to create Inventory for Product {ProductId}. Error Code: {ErrorCode}, Message: {ErrorMessage}",
                 request.ProductId, error.Code, error.Message))
            .Map(item => item.Id);
    }
}