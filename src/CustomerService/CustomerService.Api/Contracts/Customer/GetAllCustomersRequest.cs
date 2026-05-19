using CustomerService.Application.Features.Customers.Queries.GetCustomers;
using SharedKernel.Common;

namespace CustomerService.Api.Contracts.Customer;

public record GetAllCustomersRequest(string? FirstName,
    string? LastName,
    string? PhoneNumber,
    string? SortBy,
    bool IsAscending,
    int Page,
    int PageSize
)
{
    public GetCustomersQuery ToQuery()
    {
        return new GetCustomersQuery( FirstName, LastName, PhoneNumber, new PagingParams
        {
            PageSize = PageSize,
            Page = Page,
            SortBy = SortBy,
            IsAscending = IsAscending
        });
    }
}