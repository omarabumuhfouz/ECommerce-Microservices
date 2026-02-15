
using Microsoft.Extensions.Logging;

namespace ShoppingCartService.Application.Carts.Commands.ClearCart;

public class ClearCartCommandHandler : ICommandHandler<ClearCartCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Cart> _cartRepo;
    private readonly ILogger<ClearCartCommandHandler> _logger;


    public ClearCartCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<ClearCartCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _cartRepo = _unitOfWork.GetRepository<Cart>();
        _logger = logger;
    }

    async Task<Result<Unit>> IRequestHandler<ClearCartCommand, Result<Unit>>.Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Clear Cart related to Customer : {@customerId}", request.CustomerId);

        var cart = await _cartRepo.GetSingleBySpecAsync(new GetActiveCartByCustomerSpec(request.CustomerId, true), cancellationToken);
        if (cart is null) return DomainErrors.Cart.NotFoundByCustomer(request.CustomerId);


        cart.Clear();


        _cartRepo.Update(cart);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Clearing Cart Successfully related to Customer {@customerId}", request.CustomerId);

        return Unit.Value;
    }
}
