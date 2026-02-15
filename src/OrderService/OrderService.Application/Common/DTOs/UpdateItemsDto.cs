namespace OrderService.Api.Contracts.Orders;

public record UpdateItemsDto(Guid ItemId, int Quantity);