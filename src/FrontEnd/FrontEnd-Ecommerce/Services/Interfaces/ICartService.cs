
namespace FrontEnd_Ecommerce.Services.Interfaces;

public interface ICartService

{
    Task<int> GetCartCountAsync(string username);
}

public class DummyCartService : ICartService
{
    public Task<int> GetCartCountAsync(string username)
    {
        // Hardcoded to simulate items in cart
        return Task.FromResult(3);
    }
}