using CustomerService.Application.Extensions;

namespace OrderService.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .ValidateCustomerId();

        RuleFor(x => x.BillingAddressId)
            .NotEmpty().WithMessage("Billing Address ID is required.");

        RuleFor(x => x.ShippingAddressId)
            .NotEmpty().WithMessage("Shipping Address ID is required.");
        
        // Validate the List
        RuleFor(x => x.OrderItems)
            .NotEmpty().WithMessage("Order must contain at least one item.")
            .Must(items => items != null && items.Count > 0)
            .WithMessage("Order items list cannot be empty.");

        RuleForEach(x => x.OrderItems)
            .SetValidator(new OrderItemCreateDtoValidator());
    }
}