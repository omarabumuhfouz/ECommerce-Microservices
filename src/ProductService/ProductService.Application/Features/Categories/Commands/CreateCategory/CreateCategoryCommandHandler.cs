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

    async Task<Result<Guid>> IRequestHandler<CreateCategoryCommand, Result<Guid>>.Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Adding New Category with Name : {@Name}", request.Name);
        var categoryRepository = _unitOfWork.GetRepository<Category>();

        if (await categoryRepository.IsExistsAsync(c => c.Name.Value == request.Name, cancellationToken))
            return DomainErrors.Category.DuplicateName(request.Name);


        var categoryResult = Category.Create(Guid.NewGuid(), request.Name, request.Description);

        if (categoryResult.IsFailure) return categoryResult.TopError;

        await categoryRepository.AddAsync(categoryResult.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
                "Category created successfully with Id: {CategoryId}",
                categoryResult.Value.Id);

        return categoryResult.Value.Id;
    }
}
