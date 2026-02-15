namespace ShoppingCartService.Application.Carts.Commands.RemoveCartItem;

public class RemoveCartItemCommandHandler : ICommandHandler<RemoveCartItemCommand, CartDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Cart> _cartRepo;
    private readonly ICartMapper _mapper;
    private readonly ILogger<RemoveCartItemCommandHandler> _logger;

    public RemoveCartItemCommandHandler(
        IUnitOfWork unitOfWork,
        ICartMapper mapper,
        ILogger<RemoveCartItemCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _cartRepo = _unitOfWork.GetRepository<Cart>();
        _mapper = mapper;
        _logger = logger;
    }

    async Task<Result<CartDto>> IRequestHandler<RemoveCartItemCommand, Result<CartDto>>.Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling RemoveCartItemCommand for Customer {CustomerId}. Removing CartItem: {CartItemId}", 
            request.CustomerId, 
            request.CartItemId);

        var cart = await _cartRepo.GetSingleBySpecAsync(new GetActiveCartByCustomerSpec(request.CustomerId, true), cancellationToken);
        if (cart is null) return DomainErrors.Cart.NotFoundByCustomer(request.CustomerId);

        var removeResult = cart.RemoveItem(request.CartItemId);
        if (removeResult.IsFailure) 
        {
            _logger.LogWarning("Failed to remove CartItem {CartItemId}. Reason: {Error}", request.CartItemId, removeResult.TopError.Code);
            return removeResult.TopError;
        }

        _cartRepo.Update(cart);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Successfully removed CartItem {CartItemId} from Cart {CartId}", 
            request.CartItemId, 
            cart.Id);

        return await _mapper.MapToDTOAsync(cart);
    }
}
