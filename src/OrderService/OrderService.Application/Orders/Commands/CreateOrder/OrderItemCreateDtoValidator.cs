using OrderService.Api.Contracts.Orders;

namespace OrderService.Application.Orders.Commands.CreateOrder;

public class OrderItemCreateDtoValidator : AbstractValidator<CreateItemDto>
{
    public OrderItemCreateDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be at least 1.");

        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("Product name is required.");
    }
}