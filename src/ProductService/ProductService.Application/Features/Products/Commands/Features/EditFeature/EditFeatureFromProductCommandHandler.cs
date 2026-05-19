using ProductService.Domain.Products;

namespace ProductService.Application.Features.Products.Commands.Features.EditFeature;

public class EditFeatureFromProductCommandHandler : ICommandHandler<EditFeatureFromProductCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EditFeatureFromProductCommandHandler> _logger;

    public EditFeatureFromProductCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<EditFeatureFromProductCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async Task<Result<Unit>> IRequestHandler<EditFeatureFromProductCommand, Result<Unit>>.Handle(EditFeatureFromProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to edit Feature associated with Product : {@ProductId}", request.ProductId);
        
        var productRepo = _unitOfWork.GetRepository<Product>();
        var product = await productRepo.GetSingleBySpecAsync(new GetProductByIdSpec(request.ProductId, true), cancellationToken);
        if (product is null) return DomainErrors.Product.NotFound(request.ProductId);

        var editResult = product.EditFeature(request.OldName, request.NewName, request.NewValue);
        if (editResult.IsFailure) return editResult.TopError;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Feature associated with Product : {@ProductId}, updated name form {@OldName} to {@NewName}",
        request.ProductId,
        request.OldName,
        request.NewName);

        return Unit.Value;
    }
}