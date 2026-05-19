namespace ProductService.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateCategoryCommandHandler> _logger;

    public CreateCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateCategoryCommandHandler> logger
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;

    }

    public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Starting Adding New Category with Name : {@Name}", request.Name);
        var categoryRepository = _unitOfWork.GetRepository<Category>();

        bool isDuplicate = await categoryRepository.AnyAsync(c => c.Name.Value == request.Name, ct);

        if (isDuplicate) return DomainErrors.Category.DuplicateName(request.Name);

        return Category.Create(Guid.NewGuid(), request.Name, request.Description)

            .Tap(async category => await categoryRepository.AddAsync(category))

            .Tap(async _ => await _unitOfWork.SaveChangesAsync(ct))

            .Tap(category => _logger.LogInformation("Category created successfully with Id: {CategoryId}", category.Id))

            .TapError(error => _logger.LogError("Failed to create Category. Error: {ErrorMessage}", error.Message))
            .Map(category => category.Id);

    }
}
