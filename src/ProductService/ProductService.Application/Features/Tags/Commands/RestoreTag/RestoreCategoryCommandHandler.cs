using ProductService.Application.Features.Categories.Commands.RestoreCategory;
using ProductService.Application.Features.Tags.Specifications;
using ProductService.Domain.Tags;

namespace ProductService.Application.Features.Tags.Commands.RestoreTag;

public sealed class RestoreTagCommandHandler : ICommandHandler<RestoreTagCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RestoreCategoryCommandHandler> _logger;

    public RestoreTagCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<RestoreCategoryCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(RestoreTagCommand request, CancellationToken ct)
    {
        // 1. Pre-Log Intent
        _logger.LogInformation("Starting restoration process for Tag with Id: {TagId}", request.TagId);

        // Make sure to get the Tag repository
        var repository = _unitOfWork.GetRepository<Tag>();

        return await repository
            // 2. Fetch (Crucial: Uses the spec that ignores global query filters)
            .FirstOrDefaultAsync(new GetTagByIdWithDeletedSpec(request.TagId), ct)

            // 3. Null Check -> Convert to Failure if missing
            .ToResult(DomainErrors.Tag.NotFound(request.TagId))

            // 4. Execute Domain Logic
            // tag.Restore() returns Result. We map it back to the 'tag' object 
            // so the success logger down the track has access to its Name.
            .Bind(tag => tag.Restore().Map(() => tag))

            // 5. Persistence
            .Tap(tag => repository.Update(tag))
            .Tap(async tag => { await _unitOfWork.SaveChangesAsync(ct); }) // Enclosed in {} to ignore the int result

            // 6. Success Logging
            .Tap(tag => _logger.LogInformation(
                "Tag '{TagName}' with Id: {TagId} was restored successfully.",
                tag.Name, tag.Id))

            // 7. Centralized Error Logging
            // Catches NotFound, or if Restore() fails for any domain reason
            .TapError(error => _logger.LogError(
                "Failed to restore Tag with Id: {TagId}. Error Code: {ErrorCode}, Message: {ErrorMessage}",
                request.TagId, error.Code, error.Message))

            // 8. Map to MediatR's expected Unit type
            .Map(_ => Unit.Value);
    }
}