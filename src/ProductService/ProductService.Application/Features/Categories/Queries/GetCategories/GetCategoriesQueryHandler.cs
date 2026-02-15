using ProductService.Application.Features.Categories.DTOs;

namespace ProductService.Application.Features.Categories.Queries.GetCategories;

public class GetCategoriesQueryHandler : IQueryHandler<GetCategoriesQuery, List<CategoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    
    private IMapper _mapper;
    private ILogger<GetCategoriesQueryHandler> _logger;

    public GetCategoriesQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetCategoriesQueryHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    async Task<Result<List<CategoryDto>>> IRequestHandler<GetCategoriesQuery, Result<List<CategoryDto>>>.Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categoryRepo = _unitOfWork.GetRepository<Category>();
        var categories = await categoryRepo.ListAsync(new GetCategoriesSpec(), cancellationToken);
        
        if(categories is null ||!categories.Any())
        {
            _logger.LogWarning("No categories found. Or An Error occurred while retrieving categories.");
            return Enumerable.Empty<CategoryDto>().ToList();
        }

        _logger.LogInformation("Categories retrieved successfully. Count: {Count}", categories.Count);

        return _mapper.Map<List<CategoryDto>>(categories);
    }
}
