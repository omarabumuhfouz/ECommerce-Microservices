
using ProductService.Domain.Products;
using ProductService.Domain.ValueObjects;

namespace ProductService.Application.Features.Products.Commands.Features.AddFeatures;

public class AddFeaturesToProductCommandHandler : ICommandHandler<AddFeaturesToProductCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddFeaturesToProductCommandHandler> _logger;

    public AddFeaturesToProductCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<AddFeaturesToProductCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async Task<Result<Unit>> IRequestHandler<AddFeaturesToProductCommand, Result<Unit>>.Handle(AddFeaturesToProductCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Adding Features to Product with Id :{ProductId}.", request.ProductId);


        var productRepo = _unitOfWork.GetRepository<Product>();
        var product = await productRepo.FirstOrDefaultAsync(new GetProductByIdSpec(request.ProductId, true), ct)
        .ToResult(DomainErrors.Product.NotFound(request.ProductId))
        .Bind(product => PrepareFeatures(request).Map(featuresToAdding => (product, featuresToAdding)))
        .Bind(data => data.product.AddFeatures(data.featuresToAdding))
        .Tap(async _ => await _unitOfWork.SaveChangesAsync(ct))
        .Map()

        if (product == null) return DomainErrors.Product.NotFound(request.ProductId);

        var results = request.Features.Select(fd => Feature.Create(fd.Name, fd.Value)).ToList();

        var firstFailure = results.FirstOrDefault(r => r.IsFailure);
        if (firstFailure is not null) return firstFailure.TopError;

        var addingResult = product.AddFeatures(results.Select(r => r.Value).ToList());
        if (addingResult.IsFailure) return addingResult.TopError;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Added Features to Product with Id :{ProductId}. Successfully.", request.ProductId);

        return Unit.Value;
    }

    private Result<List<Feature>> PrepareFeatures(AddFeaturesToProductCommand request)
    {
        var results = request.Features.Select(fd => Feature.Create(fd.Name, fd.Value)).ToList();

        var firstFailure = results.FirstOrDefault(r => r.IsFailure);
        if (firstFailure is not null) return firstFailure.TopError;

        return results.Select(r => r.Value).ToList();

    }
}