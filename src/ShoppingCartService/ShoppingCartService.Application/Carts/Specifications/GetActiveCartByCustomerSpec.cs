
namespace ShoppingCartService.Application.Carts.Specifications;

public class GetActiveCartByCustomerSpec : Specification<Cart>
{
    public GetActiveCartByCustomerSpec(Guid customerId, bool withTracking = false) : base(c => c.CustomerId == customerId && !c.IsCheckedOut)
    {
        AddInclude(c => c.CartItems);

        if(withTracking) EnableTracking();
    }
}