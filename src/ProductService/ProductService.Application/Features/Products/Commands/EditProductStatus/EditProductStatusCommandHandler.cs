namespace ProductService.Application.Features.Products.Commands.EditProductStatus;

public class EditProductStatusCommandHandler : ICommandHandler<EditProductStatusCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EditProductStatusCommandHandler> _logger;

    public EditProductStatusCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<EditProductStatusCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async Task<Result<Unit>> IRequestHandler<EditProductStatusCommand, Result<Unit>>.Handle(EditProductStatusCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting to edit status for product with ID: {ProductId}", request.ProductId);
        
        var productRepo = _unitOfWork.GetRepository<Product>();
        var product = await productRepo.GetSingleBySpecAsync(new GetProductByIdSpec(request.ProductId, true), cancellationToken);

        if (product is null) return DomainErrors.Product.NotFound(request.ProductId);

        product.EditStatus(request.IsAvailable);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Product with ID: {ProductId} status has been changed to {IsAvailable}.", request.ProductId, request.IsAvailable);

        return Unit.Value;
    }
}
