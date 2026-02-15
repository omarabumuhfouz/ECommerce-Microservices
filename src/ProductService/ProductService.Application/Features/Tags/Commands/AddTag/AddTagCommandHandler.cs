using ProductService.Domain.TagManagement;

namespace ProductService.Application.Features.Tags.Commands.AddTag;

public class AddTagCommandHandler : ICommandHandler<AddTagCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddTagCommandHandler> _logger;

    public AddTagCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<AddTagCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async Task<Result<Guid>> IRequestHandler<AddTagCommand, Result<Guid>>.Handle(AddTagCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Adding Tag with name : {@TagName}", request.Name);

        var tagRepo = _unitOfWork.GetRepository<Tag>();

        var tagResult = Tag.Create(request.Name);
        if (tagResult.IsFailure) return tagResult.TopError;

        await tagRepo.AddAsync(tagResult.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Add Tag Successfully with Id : {@TagId} and name : {@TagName}",
            tagResult.Value.Id,
            tagResult.Value.Name);

        return tagResult.Value.Id;
    }
}