using ProductService.Application.Features.Tags.Specifications;
using ProductService.Domain.Tags;

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

    async Task<Result<Unit>> IRequestHandler<DeleteTagCommand, Result<Unit>>.Handle(DeleteTagCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Starting Deleting Tag with Id : {@TagId}", request.TagId);

        return await _unitOfWork.GetRepository<Tag>()

        .FirstOrDefaultAsync(new GetTagByIdSpec(request.TagId), ct)

        .ToResult(DomainErrors.Tag.NotFound(request.TagId))

        .Bind(tag => tag.Delete().Map(() => tag))

        .Tap(async _ => { await _unitOfWork.SaveChangesAsync(ct); })

        .Tap(tag => _logger.LogInformation(
            "Tag '{TagName}' with Id: {TagId} was successfully deleted.",
            tag.Name, tag.Id))

        .TapError(error => _logger.LogError(
            "Failed to delete Tag with Id: {TagId}. Error Code: {ErrorCode}, Message: {ErrorMessage}",
            request.TagId, error.Code, error.Message))

        .Map(_ => Unit.Value);
    }

}