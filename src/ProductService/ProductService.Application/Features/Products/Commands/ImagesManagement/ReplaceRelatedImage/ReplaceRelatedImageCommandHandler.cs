namespace ProductService.Application.Features.Products.Commands.ImagesManagement.ReplaceRelatedImage;
public class ReplaceRelatedImageCommandHandler : ICommandHandler<ReplaceRelatedImageCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ReplaceRelatedImageCommandHandler> _logger;

    public ReplaceRelatedImageCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<ReplaceRelatedImageCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async public Task<Result<Unit>> Handle(ReplaceRelatedImageCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Replacing Related Image For Product : {@ProductId}", request.ProductId);

        var productRepo = _unitOfWork.GetRepository<Product>();
        var product = await productRepo.GetSingleBySpecAsync(new GetProductByIdSpec(request.ProductId, true), cancellationToken);
        if (product is null) return DomainErrors.Product.NotFound(request.ProductId);

        var replaceResult =  product.ReplaceRelatedImage(request.OldUrl, request.NewUrl, request.NewAltText);
        if (replaceResult.IsFailure) return replaceResult.TopError;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Related image updated for product. ProductId: {ProductId}, OldUrl: {OldUrl}, NewUrl: {NewUrl}, NewAltText: {NewAltText}",
            product.Id,
            request.OldUrl,
            request.NewUrl,
            request.NewAltText);

        return Unit.Value;
    }
}
 