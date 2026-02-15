namespace ShoppingCartService.Application.Carts.Specifications;

public class GetLastCheckedOutCartSpec : Specification<Cart>
{
    public GetLastCheckedOutCartSpec(Guid customerId, bool withTracking = false) 
        :base(c => c.CustomerId == customerId && c.IsCheckedOut)

    {
        AddInclude(c => c.CartItems);
        AddOrderByDescending(c => c.CreatedAt);
        if (withTracking) EnableTracking();

    }
}