using ProductService.Domain.Products;

namespace ProductService.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteProductCommandHandler> _logger;

    public DeleteProductCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteProductCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async Task<Result<Unit>> IRequestHandler<DeleteProductCommand, Result<Unit>>.Handle(DeleteProductCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Processing deletion for product: {ProductId}", request.ProductId);

        var productRepo = _unitOfWork.GetRepository<Product>();

        return await productRepo

        .FirstOrDefaultAsync(new GetProductByIdSpec(request.ProductId), ct)

        .ToResult(DomainErrors.Product.NotFound(request.ProductId))

        .Bind(product => product.Delete().Map(() => product))

        .Tap(product => productRepo.Update(product))

        .Tap(async _ => await _unitOfWork.SaveChangesAsync(ct))

        .Tap(product => _logger.LogInformation(
                "Product '{ProductName}' with Id: {ProductId} was successfully deleted.", 
                product.Name, product.Id))
                
        .TapError(error => _logger.LogError(
                "Failed to delete Product with Id: {ProductId}. Error Code: {ErrorCode}, Message: {ErrorMessage}", 
                request.ProductId, error.Code, error.Message))

        .Map(_ => Unit.Value);
    }
}
