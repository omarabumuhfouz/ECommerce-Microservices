using ProductService.Application.Features.Products.DTOs;
using SharedKernel.Common;

namespace ProductService.Application.Features.Products.Queries.GetProducts;

public class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, PagedList<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductsQueryHandler> _logger;

    public GetProductsQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetProductsQueryHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    async Task<Result<PagedList<ProductDto>>> IRequestHandler<GetProductsQuery, Result<PagedList<ProductDto>>>.Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var productRepo = _unitOfWork.GetRepository<Product>();
        var products =  await productRepo.ListAsync(new GetProductsSpec(request.PagingParams), cancellationToken);


        if (products is null || !products.Any()) 
             return new PagedList<ProductDto>(new List<ProductDto>(), request.PagingParams.Page, request.PagingParams.PageSize, 0);

        _logger.LogInformation("Retrieved {ProductCount} products from the database.", products.Count());

        return new PagedList<ProductDto>(
            _mapper.Map<List<ProductDto>>(products),
             request.PagingParams.Page,
            request.PagingParams.PageSize,
            await productRepo.CountAsync(cancellationToken)
        );
    }
}
