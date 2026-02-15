namespace ProductService.Application.Features.Products.Commands.EditProduct;

public class EditProductCommandHandler : ICommandHandler<EditProductCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork; 
    private readonly ILogger<EditProductCommandHandler> _logger; 
 
    public EditProductCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<EditProductCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async Task<Result<Unit>> IRequestHandler<EditProductCommand, Result<Unit>>.Handle(EditProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting to edit product with ID: {ProductId}", request.ProductId);
        var productRepo = _unitOfWork.GetRepository<Product>();
        var product = await productRepo.GetSingleBySpecAsync(new GetProductByIdSpec(request.ProductId, true), cancellationToken);

        if (product is null) return DomainErrors.Product.NotFound(request.ProductId);

        if (await productRepo.IsExistsAsync(p => p.Name == request.Name, cancellationToken))
            return DomainErrors.Product.DuplicateName(request.Name);

        var editResult = product.EditMainInfo(request.Name, request.Description);
        if (editResult.IsFailure) return editResult.TopError;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Product with ID: {ProductId} has been updated.", request.ProductId);

        return Unit.Value;
    }
}
