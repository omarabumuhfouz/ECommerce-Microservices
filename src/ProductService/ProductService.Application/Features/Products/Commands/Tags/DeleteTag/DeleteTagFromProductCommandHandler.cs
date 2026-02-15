using ProductService.Domain.TagManagement;
using ProductService.Domain.TagManagement.Specifications;

namespace ProductService.Application.Features.Products.Commands.Tags.DeleteTag;

public class DeleteTagFromProductCommandHandler : ICommandHandler<DeleteTagFromProductCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteTagFromProductCommandHandler> _logger;

    public DeleteTagFromProductCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteTagFromProductCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async  Task<Result<Unit>> IRequestHandler<DeleteTagFromProductCommand, Result<Unit>>.Handle(DeleteTagFromProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Deleting Tag : {@Tag}, for product : {@Product}", request.TagId, request.ProductId);

        var productRepo = _unitOfWork.GetRepository<Product>();
        var tagRepo = _unitOfWork.GetRepository<Tag>();

        var product = await productRepo.GetSingleBySpecAsync(new GetProductByIdSpec(request.ProductId, true), cancellationToken);
        if (product is null) return DomainErrors.Product.NotFound(request.ProductId);

        var tag = await tagRepo.GetSingleBySpecAsync(new GetTagByIdSpec(request.TagId), cancellationToken);
        if (tag is null) return DomainErrors.Tag.NotFound(request.TagId);

        var removeResult = product.RemoveTag(tag);
        if (removeResult.IsFailure) return removeResult.TopError;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Tag disassociated from product. ProductId: {ProductId}, TagId: {TagId}, TagName: {TagName}",
            product.Id,
            tag.Id,
            tag.Name);

        return Unit.Value;
    }
}