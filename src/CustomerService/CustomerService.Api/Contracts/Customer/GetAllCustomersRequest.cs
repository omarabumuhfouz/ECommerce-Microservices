using CustomerService.Application.Customers.Queries.GetAllCustomers;
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
    public GetAllCustomersQuery ToQuery()
    {
        return new GetAllCustomersQuery( FirstName, LastName, PhoneNumber, new PagingParams
        {
            PageSize = PageSize,
            Page = Page,
            SortBy = SortBy,
            IsAscending = IsAscending
        });
    }
}