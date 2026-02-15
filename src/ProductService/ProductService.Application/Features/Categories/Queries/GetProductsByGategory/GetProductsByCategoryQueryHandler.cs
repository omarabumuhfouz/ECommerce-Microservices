using ProductService.Application.Features.Categories.Queries.GetProductsByGategory;
using ProductService.Application.Features.Products.DTOs;

namespace ProductService.Application.Categories.Queries;

public class GetProductsByCategoryQueryHandler : IQueryHandler<GetProductsByCategoryQuery, List<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductsByCategoryQueryHandler> _logger; 

    public GetProductsByCategoryQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetProductsByCategoryQueryHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    async Task<Result<List<ProductDto>>> IRequestHandler<GetProductsByCategoryQuery, Result<List<ProductDto>>>.Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
    {
        var categoryRepo = _unitOfWork.GetRepository<Category>();
        var productRepo = _unitOfWork.GetRepository<Product>();

        if(!await categoryRepo.IsExistsAsync(c => c.Id == request.CategoryId, cancellationToken))
                return DomainErrors.Category.NotFound(request.CategoryId);

        var products = await productRepo.ListAsync(new GetProductsByCategoryIdSpec(request.CategoryId), cancellationToken);
        if (products is null || !products.Any())
        {
            _logger.LogInformation("No products found for category ID: {CategoryId}", request.CategoryId);
            return Enumerable.Empty<ProductDto>().ToList();
        }

        _logger.LogInformation("Retrieved {ProductCount} products for category ID: {CategoryId}", products.Count, request.CategoryId);

        return  _mapper.Map<List<ProductDto>>(products);
    }
}

