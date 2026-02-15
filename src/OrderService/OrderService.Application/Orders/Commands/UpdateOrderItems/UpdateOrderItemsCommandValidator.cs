using OrderService.Api.Contracts.Orders;
using OrderService.Application.Orders.DTOs;

namespace OrderService.Application.Orders.Commands.UpdateOrderItems;

public class UpdateOrderItemsCommandValidator : AbstractValidator<UpdateOrderItemsCommand>
{
    public UpdateOrderItemsCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.OrderItems).NotEmpty();
        RuleForEach(x => x.OrderItems).SetValidator(new OrderItemUpdateDtoValidator());
    }
}

public class OrderItemUpdateDtoValidator : AbstractValidator<UpdateItemsDto>
{
    public OrderItemUpdateDtoValidator()
    {
        RuleFor(x => x.ItemId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}