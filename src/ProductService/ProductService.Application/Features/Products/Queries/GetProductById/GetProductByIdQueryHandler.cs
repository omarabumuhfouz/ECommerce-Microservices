using ProductService.Application.Features.Products.DTOs;

namespace ProductService.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductDetailsDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductByIdQueryHandler> _logger;
    public GetProductByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetProductByIdQueryHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    async Task<Result<ProductDetailsDto>> IRequestHandler<GetProductByIdQuery, Result<ProductDetailsDto>>.Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var prodcutRepo = _unitOfWork.GetRepository<Product>();
        var product = await prodcutRepo.GetSingleBySpecAsync(new GetProductByIdSpec(request.ProductId), cancellationToken);
        if (product is null) return DomainErrors.Product.NotFound(request.ProductId);

        _logger.LogInformation("Retrived Product Successfully with Id : {@ProductId}", product.Id);

        return _mapper.Map<ProductDetailsDto>(product);
    }
}
