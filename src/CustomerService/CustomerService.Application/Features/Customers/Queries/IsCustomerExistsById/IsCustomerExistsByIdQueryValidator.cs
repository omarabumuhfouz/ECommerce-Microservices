namespace CustomerService.Application.Features.Customers.Queries.IsCustomerExistsById;
    public class IsCustomerExistsByIdQueryValidator : AbstractValidator<IsCustomerExistsByIdQuery>
    {
        public IsCustomerExistsByIdQueryValidator()
        {
            RuleFor(x => x.CustomerId).ValidateCustomerId();
        }
    }
