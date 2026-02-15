namespace CustomerService.Application.Features.Customers.Queries.IsCustomerExistsByUserId;
public class IsCustomerExistsByUserIdQueryValidator : AbstractValidator<IsCustomerExistsByUserIdQuery>
{
    public IsCustomerExistsByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId).ValidateUserId();
        
    }
}