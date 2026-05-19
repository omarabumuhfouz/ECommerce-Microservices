using ProductService.Domain.Products;
using ProductService.Domain.TagManagement.Specifications;
using ProductService.Domain.Tags;

namespace ProductService.Application.Features.Products.Commands.Tags.AddTag;

public class AddTagToProductCommandHandler : ICommandHandler<AddTagToProductCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddTagToProductCommandHandler> _logger;

    public AddTagToProductCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<AddTagToProductCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async Task<Result<Unit>> IRequestHandler<AddTagToProductCommand, Result<Unit>>.Handle(AddTagToProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Add Tag : {@Tag}, for product : {@Product}", request.TagId, request.ProductId);
        
        var productRepo = _unitOfWork.GetRepository<Product>();
        var tagRepo = _unitOfWork.GetRepository<Tag>();

        var product = await productRepo.GetSingleBySpecAsync(new GetProductByIdSpec(request.ProductId, true), cancellationToken);
        if (product is null) return DomainErrors.Product.NotFound(request.ProductId);

        var tag = await tagRepo.GetSingleBySpecAsync(new GetTagByIdSpec(request.TagId), cancellationToken);
        if (tag is null) return DomainErrors.Tag.NotFound(request.TagId);

        var addResult = product.AddTag(tag);
        if (addResult.IsFailure) return addResult.TopError;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Tag associated with product. ProductId: {ProductId}, TagId: {TagId}, TagName: {TagName}",
            product.Id,
            tag.Id,
            tag.Name);

        return Unit.Value;
    }
}