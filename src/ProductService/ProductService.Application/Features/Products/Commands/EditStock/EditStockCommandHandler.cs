using ProductService.Application.Enums;
using ProductService.Domain.Constants;

namespace ProductService.Application.Features.Products.Commands.EditStock;

public class EditStockCommandHandler : ICommandHandler<EditStockCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EditStockCommandHandler> _logger;

    public EditStockCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<EditStockCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async Task<Result<Unit>> IRequestHandler<EditStockCommand, Result<Unit>>.Handle(EditStockCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting to edit stock for product with ID: {ProductId}", request.ProductId);

        var productRepo = _unitOfWork.GetRepository<Product>();
        var product = await productRepo.GetSingleBySpecAsync(new GetProductByIdSpec(request.ProductId, true), cancellationToken);

        if(product is null) return DomainErrors.Product.NotFound(request.ProductId);

        var oldStock = product.StockQuantity;

        Result updateStockResult = request.operation switch
        {
            StockOperation.Increase => product.IncreaseStock(request.Quantity),
            StockOperation.Decrease => product.DecreaseStock(request.Quantity),
            _ => DomainErrors.Product.InvalidStockOperation(request.operation.ToString())
        };

        if (updateStockResult.IsFailure) return updateStockResult.TopError;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Product stock quantity set. ProductId: {ProductId}, OldStock: {OldStock}, NewStock: {NewStock}", 
            product.Id, 
            oldStock, 
            product.StockQuantity); 

        return Unit.Value;
    }
}