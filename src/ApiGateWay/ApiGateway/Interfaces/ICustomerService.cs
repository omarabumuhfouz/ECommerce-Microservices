using ApiGateway.DTOs;

namespace ApiGateway.Interfaces;

public interface ICustomerService
{
    Task<CustomerDto> GetCustomerByUserIdOrThrowAsync(Guid userId);
}