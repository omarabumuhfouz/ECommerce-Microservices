using ProductService.Domain.Products;

namespace ProductService.Application.Features.Products.Commands.EditPrice;

public class EditPriceCommandHandler : ICommandHandler<EditPriceCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EditPriceCommandHandler> _logger;

    public EditPriceCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<EditPriceCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    async Task<Result<Unit>> IRequestHandler<EditPriceCommand, Result<Unit>>.Handle(EditPriceCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting price edit for product with Id : {@ProductId}", request.ProductId);
        
        var productRepo = _unitOfWork.GetRepository<Product>();
        var prodcut = await productRepo.GetSingleBySpecAsync(new GetProductByIdSpec(request.ProductId, true), cancellationToken);

        if (prodcut is null) return DomainErrors.Product.NotFound(request.ProductId);

        var oldPrice = prodcut.Price;

        var editResult = prodcut.EditPrice(request.NewPrice, request.NewCurrency);

        if (editResult.IsFailure) return editResult.TopError;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Product price updated. ProductId: {ProductId}, OldPrice: {OldPrice}, NewPrice: {NewPrice}",
            prodcut.Id,
            oldPrice,
            prodcut.Price);

        return Unit.Value;
    }
}