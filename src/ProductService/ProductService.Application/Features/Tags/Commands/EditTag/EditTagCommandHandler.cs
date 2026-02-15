using ProductService.Domain.TagManagement;
using ProductService.Domain.TagManagement.Specifications;

namespace ProductService.Application.Features.Tags.Commands.EditTag;

public class EditTagCommandHandler : ICommandHandler<EditTagCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EditTagCommandHandler> _logger;

    public EditTagCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<EditTagCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async Task<Result<Unit>> IRequestHandler<EditTagCommand, Result<Unit>>.Handle(EditTagCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Editing Tag with Id : {@TagId}", request.TagId);
        var tagRepo = _unitOfWork.GetRepository<Tag>();
        var tag = await tagRepo.GetSingleBySpecAsync(new GetTagByIdSpec(request.TagId), cancellationToken);
        if (tag is null) return DomainErrors.Tag.NotFound(request.TagId);


        if (await tagRepo.IsExistsAsync(t => t.Name == request.Name && t.Id != request.TagId, cancellationToken))
            return DomainErrors.Tag.NameAlreadyExists(request.Name);

        var oldName = tag.Name;

        var editResult = tag.EditName(request.Name);
        if (editResult.IsFailure) return editResult.TopError;

        tagRepo.Update(tag);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
        "Tag updated successfully. TagId: {TagId}, OldName: {OldName}, NewName: {NewName}",
        tag.Id,
        oldName,
        tag.Name);

        return Unit.Value;
    }
}