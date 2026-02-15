using ProductService.Domain.TagManagement;
using ProductService.Domain.TagManagement.Specifications;

namespace ProductService.Application.Features.Tags.Commands.DeleteTag;

public class DeleteTagCommandHandler : ICommandHandler<DeleteTagCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteTagCommandHandler> _logger;

    public DeleteTagCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteTagCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async Task<Result<Unit>> IRequestHandler<DeleteTagCommand, Result<Unit>>.Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Deleting Tag with Id : {@TagId}", request.TagId);

        var tagRepo = _unitOfWork.GetRepository<Tag>();
        var productRepo = _unitOfWork.GetRepository<Product>();

        var tag = await tagRepo.GetSingleBySpecAsync(new GetTagByIdSpec(request.TagId), cancellationToken);
        if (tag == null) return DomainErrors.Tag.NotFound(request.TagId);

        if (await productRepo.IsExistsAsync(p => p.Tags.Any(t => t.Id == request.TagId), cancellationToken))
            return DomainErrors.Tag.HasAssociatedProducts;

        tagRepo.Delete(tag);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Tag deleted successfully. TagId: {TagId}, TagName: {TagName}",
            tag.Id,
            tag.Name);

        return Unit.Value;
    }

}