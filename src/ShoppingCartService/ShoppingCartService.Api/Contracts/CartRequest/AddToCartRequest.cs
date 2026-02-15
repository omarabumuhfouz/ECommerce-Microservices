namespace ShoppingCartService.Api.Contracts.CartRequest;

public record AddToCartRequest(
    Guid CustomerId,
    Guid ProductId,
    int Quantity
);