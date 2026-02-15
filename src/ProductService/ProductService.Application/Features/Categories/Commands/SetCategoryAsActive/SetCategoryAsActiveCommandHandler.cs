using ProductService.Application.Features.Categories.Commands.SetCategoryAsActive;
using ProductService.Domain.CategoryManagement;

namespace ProductService.Application.Features.Categories.Commands.SetCategoryAsActive;

public class SetCategoryAsActiveCommandHandler : ICommandHandler<SetCategoryAsActiveCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SetCategoryAsActiveCommandHandler> _logger;

    public SetCategoryAsActiveCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<SetCategoryAsActiveCommandHandler> logger
    )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(SetCategoryAsActiveCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Starting setting category with Id: {@CategoryId} as active", request.CategoryId);
        var categoryRepository = _unitOfWork.GetRepository<Category>();

        var category = await categoryRepository.GetSingleBySpecAsync(new GetCategoryByIdSpec(request.CategoryId),ct);

        if (category is null)
            return DomainErrors.Category.NotFound(request.CategoryId);

        if (category.IsActive is true) return Unit.Value;

        category.EditStatus(true);

        categoryRepository.Update(category);
        await _unitOfWork.SaveChangesAsync(ct);

        _logger.LogInformation("Category with Id: {@CategoryId} set as active successfully", request.CategoryId);

        return Unit.Value;
    }
}
