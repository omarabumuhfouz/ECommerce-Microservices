namespace ShoppingCartService.Application.Carts.Commands.EditCartItem;

public class EditCartItemCommandHandler : ICommandHandler<EditCartItemCommand, CartDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Cart> _cartRepository;
    private readonly IValidationService _validationService;
    private readonly IProductService _productService;
    private readonly ICartMapper _mapper;
    private readonly ILogger<EditCartItemCommandHandler> _logger;

    public EditCartItemCommandHandler(
        IUnitOfWork unitOfWork,
        IValidationService validationService,
        IProductService productService,
        ICartMapper mapper,
        ILogger<EditCartItemCommandHandler> logger
        )
    {
        _unitOfWork = unitOfWork;
        _cartRepository = _unitOfWork.GetRepository<Cart>();
        _validationService = validationService;
        _mapper = mapper;
        _productService = productService; 
        _logger = logger;
    }

    async Task<Result<CartDto>> IRequestHandler<EditCartItemCommand, Result<CartDto>>.Handle(EditCartItemCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Edit CartItem related to cart item : {@cartItemId}", request.CartItemId);

        var contextResult = await GetModificationContextAsync(request, cancellationToken);
        if (contextResult.IsFailure) return contextResult.TopError;

        var (cart, product) = contextResult.Value;

        var stockResult = _validationService.EnsureSufficientStock(product, request.Quantity);
        if (stockResult.IsFailure) return stockResult.TopError;

        var updateResult = cart.UpdateItemQuantity(request.CartItemId, request.Quantity);
        if (updateResult.IsFailure) return updateResult.TopError;

        _cartRepository.Update(cart);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Updating Cart Item Successfully related to Id {@cartItemId}", request.CartItemId);

        return await _mapper.MapToDTOAsync(cart);
    }

    private async Task<Result<(Cart Cart, ProductDto Product)>> GetModificationContextAsync(
            EditCartItemCommand request,
            CancellationToken ct)
    {

        var cart = await _cartRepository.GetSingleBySpecAsync(new GetActiveCartByCustomerSpec(request.CustomerId), ct);

        if (cart is null)
            return DomainErrors.Cart.NotFoundByCustomer(request.CustomerId);

        var cartItemResult = cart.GetItemByCartItemId(request.CartItemId);
        if (cartItemResult.IsFailure) return cartItemResult.TopError;

        var productResult = await _productService
            .GetProductByIdAsync(cartItemResult.Value.ProductId);

        if (productResult.IsFailure) return productResult.TopError;

        return (cart, productResult.Value);
    }

}
