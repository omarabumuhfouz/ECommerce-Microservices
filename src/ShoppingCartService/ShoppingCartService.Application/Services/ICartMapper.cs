namespace ShoppingCartService.Application.Services;

public interface ICartMapper
{
    Task<Result<CartDto>> MapToDTOAsync(Cart cart);
}
