
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Common;

namespace CustomerService.Application.Customers.Queries.GetAllCustomers;

public record GetAllCustomersQuery(
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    PagingParams PagingParams

    ) : IQuery<PagedList<CustomerDto>>;
