namespace ShoppingCartService.Application.Carts.Commands.AddToCart;

public class AddToCartCommandHandler : ICommandHandler<AddToCartCommand, CartDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Cart> _cartRepo;
    private readonly IProductService _productService;
    private readonly IValidationService _validationService;
    private readonly ICartMapper _mapper;
    private readonly ILogger<AddToCartCommandHandler> _logger;

    public AddToCartCommandHandler(
        IUnitOfWork unitOfWork,
        IProductService productService,
        IValidationService validationService,
        ICartMapper mapper,
        ILogger<AddToCartCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _cartRepo = _unitOfWork.GetRepository<Cart>();
        _productService = productService;
        _validationService = validationService;
        _mapper = mapper;
        _logger = logger;
    }

    async Task<Result<CartDto>> IRequestHandler<AddToCartCommand, Result<CartDto>>.Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
                "Adding item to cart for Customer {CustomerId}. ProductId: {ProductId}, Quantity: {Quantity}", 
                request.CustomerId, 
                request.ProductId, 
                request.Quantity);

        var contextResult = await GetAddToCartContextAsync(request, cancellationToken);
        if (contextResult.IsFailure) return contextResult.TopError;

        var ( product, cart) = contextResult.Value;

        if (cart is null)
        {
            cart = Cart.Create(request.CustomerId).Value;

            await _cartRepo.AddAsync(cart);
        }

        var cartitemsQuantity = cart.CartItems.Count();

        var currentQuantityInCart = GetCurrentQuantity(cart, product.Id);
        var totalQuantity = currentQuantityInCart + request.Quantity;

        var stockResult =_validationService.EnsureSufficientStock(product, totalQuantity);
        if (stockResult.IsFailure) return stockResult.TopError;

        var addResult = cart.AddItem(
            product.Id,
            request.Quantity,
            product.Price,
            product.CalculateDiscount());

        if(addResult.IsFailure) return addResult.TopError;

        // _cartRepo.Update(cart);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Map the cart entity to the DTO, which includes price calculations.
        return await _mapper.MapToDTOAsync(cart!);
    }

    private async Task<Result<( ProductDto, Cart?)>> GetAddToCartContextAsync(
        AddToCartCommand request,
        CancellationToken cancellationToken
    )
    {
        var productResult = await _productService.GetProductByIdAsync(request.ProductId);
        if (productResult.IsFailure) return productResult.TopError;

        // Retrieve an active cart for the customer (include related CartItems and Products).
        Cart? cart = await _cartRepo.GetSingleBySpecAsync(new GetActiveCartByCustomerSpec(request.CustomerId, true), cancellationToken);

        return (productResult.Value, cart);
    }

    private int GetCurrentQuantity(Cart cart, Guid productId)
    {
        var itemResult = cart.GetItemByProductId(productId);

        // If Success, return quantity. If Failure (NotFound), return 0.
        return itemResult.IsSuccess ? itemResult.Value.Quantity.Value : 0;
    }
}
