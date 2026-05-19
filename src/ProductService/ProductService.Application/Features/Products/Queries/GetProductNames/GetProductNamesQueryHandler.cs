using ProductService.Application.Features.Products.DTOs;
using ProductService.Domain.Products;

namespace ProductService.Application.Features.Products.Queries.GetProductNames;

public class GetProductNamesQueryHandler : IQueryHandler<GetProductNamesQuery, List<ProductNameDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetProductNamesQueryHandler> _logger;

    public GetProductNamesQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetProductNamesQueryHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }


    async Task<Result<List<ProductNameDto>>> IRequestHandler<GetProductNamesQuery, Result<List<ProductNameDto>>>.Handle(GetProductNamesQuery request, CancellationToken cancellationToken)
    {
        var productRepo = _unitOfWork.GetRepository<Product>();
        var products = await productRepo.ListAsync(new GetProductsNamesSpec(request.ProductIds), cancellationToken);

        if (products is null || !products.Any()) return new List<ProductNameDto>();

        var foundIds = products.Select(p => p.Id).ToHashSet();
        var missingIds = request.ProductIds.Where(id => !foundIds.Contains(id)).ToList();

        if (missingIds.Any()) return DomainErrors.Product.MissingIds(missingIds);

        _logger.LogInformation("Retrieved {ProductCount} product names from the database.", products.Count);

        return products;
    }
}
