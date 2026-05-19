using ProductService.Domain.Products;

namespace ProductService.Application.Features.Products.Commands.AddDiscount;

public class AddDiscountToProductCommandHandler : ICommandHandler<AddDiscountToProductCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddDiscountToProductCommandHandler> _logger;

    public AddDiscountToProductCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<AddDiscountToProductCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async Task<Result<Unit>> IRequestHandler<AddDiscountToProductCommand, Result<Unit>>.Handle(AddDiscountToProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting to add discount to product with Id : {@ProductId}", request.ProductId);

        var productRepo = _unitOfWork.GetRepository<Product>();

        var product =  await productRepo.GetSingleBySpecAsync(new GetProductByIdSpec(request.ProductId, true), cancellationToken); ;

        if (product is null) return DomainErrors.Product.NotFound(request.ProductId);

        var editResult = product.EditDiscount(request.DiscountPercentage, request.DiscountEndDate);

        if (editResult.IsFailure) return editResult.TopError;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Add Discount Percentage : {@Percentage} to Product Successfully with Id : {@ProductId}",
            request.DiscountPercentage,
            request.ProductId);

        return Unit.Value;
    }
}