using ProductService.Domain.Products;

namespace ProductService.Application.Features.Products.Commands.Features.DeleteFeature;

public class DeleteFeatureFromProductCommandHandler : ICommandHandler<DeleteFeatureFromProductCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteFeatureFromProductCommandHandler> _logger;

    public DeleteFeatureFromProductCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteFeatureFromProductCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async Task<Result<Unit>> IRequestHandler<DeleteFeatureFromProductCommand, Result<Unit>>.Handle(DeleteFeatureFromProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting to delete Feature with name :{@FeatureName}, from Product : {@ProductId}",request.Name, request.ProductId);

        var productRepo = _unitOfWork.GetRepository<Product>();
        var product = await productRepo.GetSingleBySpecAsync(new GetProductByIdSpec(request.ProductId, true), cancellationToken);
        if (product is null) return DomainErrors.Product.NotFound(request.ProductId);

        var removeResult = product.RemoveFeature(request.Name);
        if (removeResult.IsFailure) return removeResult.TopError;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Delete Feature with name :{@FeatureName}, accosiated with Product : {@ProductId}",
                request.Name,
                request.ProductId);

        return Unit.Value;
    }
}