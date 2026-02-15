using ProductService.Application.Features.Categories.DTOs;

namespace ProductService.Application.Features.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, CategoryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCategoryByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    async Task<Result<CategoryDto>> IRequestHandler<GetCategoryByIdQuery, Result<CategoryDto>>.Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var categoryRepo = _unitOfWork.GetRepository<Category>();
        var category = await categoryRepo.GetSingleBySpecAsync(new GetCategoryByIdSpec(request.CategoryId));

        if (category is null) return DomainErrors.Category.NotFound(request.CategoryId);

        return _mapper.Map<CategoryDto>(category);
    }
}
