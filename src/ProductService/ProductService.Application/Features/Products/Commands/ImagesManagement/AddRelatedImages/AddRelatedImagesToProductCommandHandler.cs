using ProductService.Domain.Products;
using ProductService.Domain.ValueObjects;

namespace ProductService.Application.Features.Products.Commands.ImagesManagement.AddRelatedImages;

public class AddRelatedImagesToProductCommandHandler : ICommandHandler<AddRelatedImagesToProductCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddRelatedImagesToProductCommandHandler> _logger;

    public AddRelatedImagesToProductCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<AddRelatedImagesToProductCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async Task<Result<Unit>> IRequestHandler<AddRelatedImagesToProductCommand, Result<Unit>>.Handle(AddRelatedImagesToProductCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Starting Adding Related Images to product : {ProductId}", request.ProductId);


        var productRepo = _unitOfWork.GetRepository<Product>();

        var product = await productRepo.FirstOrDefaultAsync(new GetProductByIdSpec(request.ProductId, true), ct);
        if (product is null) return DomainErrors.Product.NotFound(request.ProductId);

        var results = request.RelatedImages.Select(ri => Image.Create(ri.Url, ri.AltText)).ToList();
        var firstFailure = results.FirstOrDefault(x => x.IsFailure);
        if( firstFailure is not null) return firstFailure.TopError;

        var addResult = product.AddRelatedImages(results.Select(r => r.Value).ToList());
        if (addResult.IsFailure) return addResult.TopError;

        await _unitOfWork.SaveChangesAsync(ct);

        _logger.LogInformation(
        "Successfully added {ImageCount} related images to product {ProductId}",
        request.RelatedImages.Count, 
        product.Id);

        return Unit.Value;
    }
}