namespace ShoppingCartService.Api.Contracts.CartRequest;

public record EditCartItemRequest(Guid CustomerId,int Quantity);