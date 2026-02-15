
using SharedKernel.Common;

namespace ShoppingCartService.Application.Carts.Commands.RestoreCart;

public class RestoreCartCommandHandler : ICommandHandler<RestoreCartCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public RestoreCartCommandHandler(IUnitOfWork unitOfWork)
         => _unitOfWork = unitOfWork;


    Task<Result<Unit>> IRequestHandler<RestoreCartCommand, Result<Unit>>.Handle(RestoreCartCommand request, CancellationToken ct)
    {
        return _unitOfWork.GetRepository<Cart>()
        .GetSingleBySpecAsync(new GetLastCheckedOutCartSpec(request.CustomerId, true), ct)
        .ToResult(DomainErrors.Cart.NoCheckedOutCartToRestore(request.CustomerId))
        .Tap(cart => cart.Restore())
        .Tap(async _ => await _unitOfWork.SaveChangesAsync(ct))
        .Map(_ => Unit.Value);
    }
}