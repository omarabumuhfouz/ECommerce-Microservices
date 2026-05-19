using ProductService.Application.Features.Products.DTOs;
using ProductService.Application.Features.Products.Queries.GetProductsByTagId;

namespace ProductService.Application.Features.Tags.Queries.GetProductsByTagIdQuery;

public sealed class GetProductsByTagIdQueryHandler : ICommandHandler<GetProductsByTagIdQuery, List<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetProductsByTagIdQueryHandler> _logger;

    public GetProductsByTagIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetProductsByTagIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<List<ProductDto>>> Handle(GetProductsByTagIdQuery request, CancellationToken ct)
    {
        var products = await _unitOfWork.GetRepository<Product>().GetListAsync(new GetProductsByTagIdSpec(request.TagId), ct);

        return Result.Success(products ?? [])
            .Tap(list => _logger.LogInformation(
                "Retrieved {Count} products for TagId: {TagId}", list.Count, request.TagId));
    }
}