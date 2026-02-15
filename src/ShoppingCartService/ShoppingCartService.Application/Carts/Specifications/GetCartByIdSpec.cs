using System.ComponentModel;

namespace ShoppingCartService.Application.Carts.Specifications;
public class GetCartByIdSpec : Specification<Cart>
{
    public GetCartByIdSpec(Guid cartId, bool withTracking = false) : base(c => c.Id == cartId)
    {
        AddInclude(c => c.CartItems);

        if (withTracking) EnableTracking();
    }
}