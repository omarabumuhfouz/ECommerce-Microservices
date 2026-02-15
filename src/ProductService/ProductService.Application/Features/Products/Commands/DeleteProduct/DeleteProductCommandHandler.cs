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

    async Task<Result<Unit>> IRequestHandler<DeleteProductCommand, Result<Unit>>.Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing deletion for product: {ProductId}", request.ProductId);
        var productRepo = _unitOfWork.GetRepository<Product>();

        var product = await productRepo.GetSingleBySpecAsync(new GetProductByIdSpec(request.ProductId, true), cancellationToken);

        if (product is null) return DomainErrors.Product.NotFound(request.ProductId);

        product.EditStatus(false);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Product with ID: {ProductId} has been soft-deleted.", request.ProductId);

        return Unit.Value;
    }
}
