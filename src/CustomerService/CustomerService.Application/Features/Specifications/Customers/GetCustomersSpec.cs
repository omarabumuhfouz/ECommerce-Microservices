using Ardalis.Specification;
using CustomerService.Domain.Customers;
using SharedKernel.Common;

namespace CustomerService.Application.Features.Specifications.Customers;

public sealed class GetCustomersSpec : Specification<Customer>
{
    public GetCustomersSpec(
        string? firstName,
        string? lastName,
        string? phoneNumber,
        PagingParams pagingParams)
    {
        Query.Where(c => c.FullName.FirstName.Contains(firstName!), !string.IsNullOrEmpty(firstName))
             .Where(c => c.FullName.LastName.Contains(lastName!), !string.IsNullOrEmpty(lastName))
             .Where(c => c.PhoneNumber.Value.Contains(phoneNumber!), !string.IsNullOrEmpty(phoneNumber));

        Query.Include(c => c.Addresses);

        ApplyOrdering(pagingParams);

        Query.Skip(pagingParams.Skip)
             .Take(pagingParams.PageSize);
    }

    private void ApplyOrdering(PagingParams pagingParams)
    {
        var sortBy = pagingParams.SortBy?.ToLower();

        if (pagingParams.IsAscending)
        {
            _ = sortBy switch
            {
                CustomerConstants.SEARCH_BY_LAST_NAME => Query.OrderBy(c => c.FullName.LastName),
                CustomerConstants.SEARCH_BY_PHONE_NUMBER => Query.OrderBy(c => c.PhoneNumber.Value),
                _ => Query.OrderBy(c => c.FullName.FirstName)
            };
        }
        else
        {
            _ = sortBy switch
            {
                CustomerConstants.SEARCH_BY_LAST_NAME => Query.OrderByDescending(c => c.FullName.LastName),
                CustomerConstants.SEARCH_BY_PHONE_NUMBER => Query.OrderByDescending(c => c.PhoneNumber.Value),
                _ => Query.OrderByDescending(c => c.FullName.FirstName)
            };
        }
    }
}