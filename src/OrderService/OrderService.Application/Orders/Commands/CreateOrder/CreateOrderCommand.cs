using OrderService.Api.Contracts.Orders;

namespace OrderService.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand(
    Guid CustomerId,
    Guid BillingAddressId,
    Guid ShippingAddressId,
    List<CreateItemDto> OrderItems
)
: ICommand<Guid>;