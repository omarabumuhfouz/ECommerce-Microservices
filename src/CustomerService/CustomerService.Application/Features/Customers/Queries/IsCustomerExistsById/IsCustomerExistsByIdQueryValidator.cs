using CustomerService.Application.Customers.Queries;

using CustomerService.Application.Extensions;

namespace CustomerService.Application.Customers.Validators;
    public class IsCustomerExistsByIdQueryValidator : AbstractValidator<IsCustomerExistsByIdQuery>
    {
        public IsCustomerExistsByIdQueryValidator()
        {
            RuleFor(x => x.CustomerId).ValidateCustomerId();
        }
    }
