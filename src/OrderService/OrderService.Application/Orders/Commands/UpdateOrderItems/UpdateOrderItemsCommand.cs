using OrderService.Api.Contracts.Orders;
using OrderService.Application.Orders.DTOs;

namespace OrderService.Application.Orders.Commands.UpdateOrderItems;

public record UpdateOrderItemsCommand(
    Guid OrderId,
    List<UpdateItemsDto> OrderItems
    ) : ICommand<Unit>;