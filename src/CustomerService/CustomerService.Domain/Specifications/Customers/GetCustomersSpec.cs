using System.Linq.Expressions;
using CustomerService.Domain.Constants;
using CustomerService.Domain.Entities;
using SharedKernel.Common;
using SharedKernel.Specifications;

namespace CustomerService.Domain.Specifications.Customers;

public class GetCustomersSpec : Specification<Customer>
{
    public GetCustomersSpec(
            string? firstName,
            string? lastName,
            string? phoneNumber,
            PagingParams pagingParams) 
             : base(
                c => (string.IsNullOrEmpty(firstName) || c.FullName.FirstName.ToLower().Contains(firstName.ToLower())) &&
                     (string.IsNullOrEmpty(lastName) || c.FullName.LastName.ToLower().Contains(lastName.ToLower())) &&
                     (string.IsNullOrEmpty(phoneNumber) || c.PhoneNumber.Value.ToLower().Contains(phoneNumber.ToLower())))
    {

        Expression<Func<Customer, object>> keySelector = pagingParams.SortBy?.ToLower() switch
        {
            CustomerConstants.SEARCH_BY_FIRST_NAME => c => c.FullName.FirstName,
            CustomerConstants.SEARCH_BY_LAST_NAME => c => c.FullName.LastName,
            CustomerConstants.SEARCH_BY_PHONE_NUMBER => c => c.PhoneNumber.Value,
            _ => c => c.FullName.FirstName
        };

        if(pagingParams.IsAscending) AddOrderBy(keySelector);
        else                         AddOrderByDescending(keySelector);


        AddInclude(c => c.Addresses);
        ApplyPaging(pagingParams.Skip, pagingParams.PageSize);
    }
}