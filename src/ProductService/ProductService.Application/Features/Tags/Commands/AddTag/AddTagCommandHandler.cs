using ProductService.Domain.Tags;

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

    async Task<Result<Guid>> IRequestHandler<AddTagCommand, Result<Guid>>.Handle(AddTagCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Starting Adding Tag with name : {@TagName}", request.Name);

        var tagRepo = _unitOfWork.GetRepository<Tag>();

        return Tag.Create(request.Name)

        .Tap(async tag => await tagRepo.AddAsync(tag, ct))

        .Tap(async _ => await _unitOfWork.SaveChangesAsync(ct))

        .Tap(tag => _logger.LogInformation(
                "Add Tag Successfully with Id : {@TagId} and name : {@TagName}",
                tag.Id, tag.Name))

        .TapError(error => _logger.LogError(
                "Failed to create Tag '{TagName}'. Error Code: {ErrorCode}, Message: {ErrorMessage}",
                request.Name, error.Code, error.Message))

        .Map(tag => tag.Id);
    }
}