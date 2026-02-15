namespace FrontEnd_Ecommerce.Services.Interfaces;

public interface IRequestRetryHandler
{
    Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> action);
}


