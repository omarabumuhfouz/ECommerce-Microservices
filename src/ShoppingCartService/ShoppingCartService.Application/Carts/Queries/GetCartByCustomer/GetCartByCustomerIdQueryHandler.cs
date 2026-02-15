namespace ShoppingCartService.Application.Carts.Queries.GetCartByCustomer;

public class GetCartByCustomerIdQueryHandler : IQueryHandler<GetCartByCustomerIdQuery, CartDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICartMapper _mapper;
    private readonly IRepository<Cart> _cartRepo;

    public GetCartByCustomerIdQueryHandler(
        IUnitOfWork unitOfWork,
        ICartMapper mapper
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cartRepo = unitOfWork.GetRepository<Cart>();
    }

    async Task<Result<CartDto>> IRequestHandler<GetCartByCustomerIdQuery, Result<CartDto>>.Handle(GetCartByCustomerIdQuery request, CancellationToken ct)
    {

        var cart = await _cartRepo
                        .GetSingleBySpecAsync(new GetActiveCartByCustomerSpec(request.CustomerId), ct);

        if (cart == null)
        {
            cart = Cart.Create(request.CustomerId).Value;
            await _cartRepo.AddAsync(cart);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        return  await _mapper.MapToDTOAsync(cart);
    }
}
