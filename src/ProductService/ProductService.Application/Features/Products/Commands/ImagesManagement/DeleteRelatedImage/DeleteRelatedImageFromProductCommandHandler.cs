using ProductService.Domain.Products;

namespace ProductService.Application.Features.Products.Commands.ImagesManagement.DeleteRelatedImage;

public class DeleteRelatedImageFromProductCommandHandler : ICommandHandler<DeleteRelatedImageFromProductCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteRelatedImageFromProductCommandHandler> _logger;

    public DeleteRelatedImageFromProductCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteRelatedImageFromProductCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async Task<Result<Unit>> IRequestHandler<DeleteRelatedImageFromProductCommand, Result<Unit>>.Handle(DeleteRelatedImageFromProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Deleting Related Image from product : {ProductId}", request.ProductId);

        var productRepo = _unitOfWork.GetRepository<Product>();
        var product = await productRepo.GetSingleBySpecAsync(new GetProductByIdSpec(request.ProductId, true), cancellationToken);
        if (product is null) return DomainErrors.Product.NotFound(request.ProductId);

        var deleteResult = product.RemoveRelatedImage(request.Url);
        if (deleteResult.IsFailure) return deleteResult.TopError;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Related image removed from product. ProductId: {ProductId}, ImageUrl: {ImageUrl}", 
            product.Id, 
            request.Url);

        return Unit.Value;
    }
}