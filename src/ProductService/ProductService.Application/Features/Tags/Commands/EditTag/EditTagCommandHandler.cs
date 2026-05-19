using ProductService.Application.Features.Tags.Specifications;
using ProductService.Domain.TagManagement.Specifications;
using ProductService.Domain.Tags;

namespace ProductService.Application.Features.Tags.Commands.EditTag;

public class EditTagCommandHandler : ICommandHandler<EditTagCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EditTagCommandHandler> _logger;

    public EditTagCommandHandler(IUnitOfWork unitOfWork, ILogger<EditTagCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async Task<Result<Unit>> IRequestHandler<EditTagCommand, Result<Unit>>.Handle(EditTagCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Starting Editing Tag with Id : {@TagId}", request.TagId);

        var tagRepo = _unitOfWork.GetRepository<Tag>();
        var tag = await tagRepo.FirstOrDefaultAsync(new GetTagByIdSpec(request.TagId), ct);
        if (tag is null) return DomainErrors.Tag.NotFound(request.TagId);


        if (await tagRepo.AnyAsync(t => t.Name == request.Name && t.Id != request.TagId, ct))
            return DomainErrors.Tag.NameAlreadyExists(request.Name);

        var oldName = tag.Name;

        return tag.EditName(request.Name)

        .Tap(_ => tagRepo.Update(tag))

        .Tap(async _ => { await _unitOfWork.SaveChangesAsync(ct); })

        .Tap(_ => _logger.LogInformation(
            "Tag with Id: {TagId} was successfully updated from '{OldName}' to '{NewName}'.", 
            request.TagId, oldName, request.Name))
                
        .TapError(error => _logger.LogError(
            "Failed to edit Tag with Id: {TagId}. Error Code: {ErrorCode}, Message: {ErrorMessage}", 
            request.TagId, error.Code, error.Message))

       .Map(_ => Unit.Value);
    }
}