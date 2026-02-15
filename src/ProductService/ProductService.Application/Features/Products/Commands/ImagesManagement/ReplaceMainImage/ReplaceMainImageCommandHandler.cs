namespace ProductService.Application.Features.Products.Commands.ImagesManagement.ReplaceMainImage;

public class ReplaceMainImageCommandHandler : ICommandHandler<ReplaceMainImageCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ReplaceMainImageCommandHandler> _logger;

    public ReplaceMainImageCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<ReplaceMainImageCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async Task<Result<Unit>> IRequestHandler<ReplaceMainImageCommand, Result<Unit>>.Handle(ReplaceMainImageCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Replacing Main image for Product : {@ProductId}", request.ProductId);

        var productRepo = _unitOfWork.GetRepository<Product>();
        var product = await productRepo.GetSingleBySpecAsync(new GetProductByIdSpec(request.ProductId, true), cancellationToken);
        if (product is null) return DomainErrors.Product.NotFound(request.ProductId);

        var oldImageUrl = product.MainImage.Url;

        var replaceResult = product.ReplaceMainImage(request.NewMainUrl, request.NewTextAlt);
        if (replaceResult.IsFailure) return replaceResult.TopError;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Product main image replaced. ProductId: {ProductId}, OldUrl: {OldUrl}, NewUrl: {NewUrl}, NewAltText: {NewAltText}",
            product.Id,
            oldImageUrl,
            request.NewMainUrl,
            request.NewTextAlt);

        return Unit.Value;
    }
}