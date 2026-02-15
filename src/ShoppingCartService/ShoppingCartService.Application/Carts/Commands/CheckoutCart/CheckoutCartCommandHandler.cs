namespace ShoppingCartService.Application.Carts.Commands.CheckoutCart;

public class CheckoutCartCommandHandler : ICommandHandler<CheckoutCartCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CheckoutCartCommandHandler> _logger;

    public CheckoutCartCommandHandler(IUnitOfWork unitOfWork, ILogger<CheckoutCartCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(CheckoutCartCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Checking out cart for Customer: {CustomerId}", request.CustomerId);
        
        var cartRepo = _unitOfWork.GetRepository<Cart>();
        var cart = await cartRepo.GetSingleBySpecAsync(new GetActiveCartByCustomerSpec(request.CustomerId, true), ct);

        if (cart is null) return DomainErrors.Cart.NotFoundByCustomer(request.CustomerId);

        var checkoutResult = cart.Checkout();
        if (checkoutResult.IsFailure) return checkoutResult.TopError;


        cartRepo.Update(cart);
        await _unitOfWork.SaveChangesAsync(ct);

        _logger.LogInformation("Successfully checked out Cart: {CartId} for Customer: {CustomerId}", cart.Id, request.CustomerId);

        return Unit.Value;
    }
}
