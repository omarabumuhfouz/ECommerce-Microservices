using SharedKernel.Primitives.Results;

namespace OrderService.Application.Services;

public interface ICustomerService
{
    public Task<Result<CustomerDto>> GetCustomerByIdAsync(Guid customerId);

}
