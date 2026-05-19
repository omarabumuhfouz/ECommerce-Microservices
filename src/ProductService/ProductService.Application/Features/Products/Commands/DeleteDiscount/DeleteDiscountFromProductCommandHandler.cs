using ProductService.Domain.Products;

namespace ProductService.Application.Features.Products.Commands.DeleteDiscount;

public class DeleteDiscountFromProductCommandHandler : ICommandHandler<DeleteDiscountFromProductCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteDiscountFromProductCommandHandler> _logger;

    public DeleteDiscountFromProductCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteDiscountFromProductCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async Task<Result<Unit>> IRequestHandler<DeleteDiscountFromProductCommand, Result<Unit>>.Handle(DeleteDiscountFromProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing discount removal for product: {ProductId}", request.ProductId);
        var productRepo = _unitOfWork.GetRepository<Product>();

        var product = await productRepo.GetSingleBySpecAsync(new GetProductByIdSpec(request.ProductId, true), cancellationToken);

        if (product is null) return DomainErrors.Product.NotFound(request.ProductId);

        var oldDiscountPercentage = product.Discount.Percentage;

        if (oldDiscountPercentage == 0)
        {
            _logger.LogInformation(
                "Product discount removal requested, but no discount was active. ProductId: {ProductId}",
                product.Id);

            return Unit.Value; // No discount to remove 
        }

         product.RemoveDiscount();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Product discount removed. ProductId: {ProductId}, OldDiscountPercentage: {OldDiscountPercentage}",
            product.Id,
            oldDiscountPercentage);

        return Unit.Value;
    }
}