using Contracts.Customer;
using Grpc.Core;
using OrderService.Application.Services;
using OrderService.Domain.DTOs;
using OrderService.Domain.Errors;
using SharedKernel.Primitives.Results;

namespace OrderService.Infrastructure.Services;

public class GrpcCustomerService : ICustomerService
{
    private readonly CustomerProtoService.CustomerProtoServiceClient _customerClient;

    public GrpcCustomerService(CustomerProtoService.CustomerProtoServiceClient customerClient)
    {
        _customerClient = customerClient;
    }

    public async Task<Result<CustomerDto>> GetCustomerByIdAsync(Guid customerId)
    {
        try
        {
            var customer = await _customerClient.GetByIdAsync(new GetCustomerByIdRequest { CustomerId = customerId.ToString() });
            if (customer is null) return DomainErrors.Customer.NotFound(customerId);

            return CustomerDto.FromGrpcModel(customer);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
        {
            return DomainErrors.Customer.NotFound(customerId);
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.DeadlineExceeded)
        {
            return DomainErrors.System.RemoteTimeout;
        }
        catch (RpcException)
        {
            return DomainErrors.System.InternalError;
        }
    }
}
