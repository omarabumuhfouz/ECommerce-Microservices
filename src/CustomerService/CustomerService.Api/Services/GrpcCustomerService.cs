using Contracts.Customer;
using CustomerService.Application.Customers.Commands.AddCustomer;
using CustomerService.Application.Customers.Commands.EditCustomer;
using CustomerService.Application.Customers.Queries;
using Grpc.Core;
using MediatR;
using SharedKernel.Extensions;

namespace CustomerService.Api.Services;

public class GrpcCustomerService : CustomerProtoService.CustomerProtoServiceBase
{
    private readonly ISender _sender;

    public GrpcCustomerService(ISender sender)
    {
        _sender = sender;
    }

    public override async Task<AddCustomerResponse> AddCustomer(AddCustomerRequest request, ServerCallContext context)
    {
        var command = new AddCustomerCommand(
            Guid.Parse(request.UserId),
            request.FirstName,
            request.LastName,
            request.PhoneNumber);

        // 1. Send Command
        var result = await _sender.Send(command);

        // 2. Unwrap or Throw (This replaces the entire if/else/try/catch block)
        var customerId = result.ValueOrThrowRpc();

        // 3. Return Success (We only reach here if no exception was thrown)
        return new AddCustomerResponse
        {
            CustomerId = customerId.ToString(),
            Success = true,
            Message = "Added Customer Successfully"
        };
    }
   
    public override async Task<CustomerModel> GetById(GetCustomerByIdRequest request, ServerCallContext context)
    {
        var query = new GetCustomerByIdQuery(Guid.Parse(request.CustomerId));
        
        var result = await _sender.Send(query);

        // Unwrap or Throw
        var customer = result.ValueOrThrowRpc();

        // Map Domain Object to Proto Object
        var response = new CustomerModel
        {
            Id = customer.Id.ToString(),
            UserId = customer.UserId.ToString(),
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            PhoneNumber = customer.PhoneNumber,
        };

        if (customer.Addresses is not null)
        {
            response.Addresses.AddRange(customer.Addresses.Select(a => new AddressModel
            {
                Id = a.AddressId.ToString(),
                AddressLine1 = a.AddressLine1,
                City = a.City,
                Country = a.Country
                // ... map other fields
            }));
        }

        return response;
    }

    public override async Task<UpdateCustomerResponse> UpdateCustomer(UpdateCustomerRequest request, ServerCallContext context)
    {
        var command = new EditCustomerCommand(
            Guid.Parse(request.UserId),
            request.FirstName,
            request.LastName,
            request.PhoneNumber);

        var result = await _sender.Send(command);

        // For void commands, we just check for failure
        result.ThrowRpcIfFailure();

        return new UpdateCustomerResponse { Success = true };
    }
}