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
        return await _unitOfWork.GetRepository<Category>()

        .FirstOrDefaultAsync(new GetCategoryByIdSpec(request.CategoryId))

        .ToResult(DomainErrors.Category.NotFound(request.CategoryId))

        .Map(category => _mapper.Map<CategoryDto>(category));
    }
}
