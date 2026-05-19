using SharedKernel.Common;

namespace CustomerService.Application.Features.Customers.Queries.GetCustomers;

public record GetCustomersQuery(
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    PagingParams PagingParams

    ) : IQuery<PagedList<CustomerDto>>;
