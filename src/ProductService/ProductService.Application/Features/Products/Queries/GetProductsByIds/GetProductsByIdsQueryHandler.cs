using ProductService.Application.Features.Products.DTOs;

namespace ProductService.Application.Features.Products.Queries.GetProductsByIds;

public class GetProductsByIdsQueryHandler : IQueryHandler<GetProductsByIdsQuery, List<ProductDetailsDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductsByIdsQueryHandler> _logger;

    public GetProductsByIdsQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetProductsByIdsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<List<ProductDetailsDto>>> Handle(GetProductsByIdsQuery request, CancellationToken cancellationToken)
    {
        var productRepo = _unitOfWork.GetRepository<Product>();
        var products = await productRepo.ListAsync(new GetProductsByIdsSpec(request.ProductIds), cancellationToken);

        if (products is null || !products.Any())
        {
            // If no products found at all, return empty list or error depending on strictness.
            // Here we return empty list if the input list was empty or nothing matched (though validator checks input).
            // If we want to enforce that IDs must exist, we check below.
            if (request.ProductIds.Any())
            {
                 return DomainErrors.Product.MissingIds(request.ProductIds);
            }
            return new List<ProductDetailsDto>();
        }

        var foundIds = products.Select(p => p.Id).ToHashSet();
        var missingIds = request.ProductIds.Where(id => !foundIds.Contains(id)).ToList();

        if (missingIds.Any())
        {
            return DomainErrors.Product.MissingIds(missingIds);
        }

        _logger.LogInformation("Retrieved {ProductCount} products successfully.", products.Count);

        return _mapper.Map<List<ProductDetailsDto>>(products);
    }
}
