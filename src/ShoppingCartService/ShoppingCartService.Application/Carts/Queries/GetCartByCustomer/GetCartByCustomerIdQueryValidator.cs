namespace ShoppingCartService.Application.Carts.Queries.GetCartByCustomer;

public class GetCartByCustomerIdQueryValidator : AbstractValidator<GetCartByCustomerIdQuery>
{
    public GetCartByCustomerIdQueryValidator()
    {
        RuleFor(c => c.CustomerId)
            .ValidateCustomerId();
        
    }
}