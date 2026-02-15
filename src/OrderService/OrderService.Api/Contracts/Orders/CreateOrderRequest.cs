using OrderService.Application.Orders.DTOs;

namespace OrderService.Api.Contracts.Orders;

public record CreateOrderRequest(
    Guid CustomerId,
    Guid BillingAddressId,
    Guid ShippingAddressId,
    List<CreateItemDto> OrderItems);