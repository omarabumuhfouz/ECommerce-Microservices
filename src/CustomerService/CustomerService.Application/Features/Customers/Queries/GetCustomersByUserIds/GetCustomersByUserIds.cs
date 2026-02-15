
namespace CustomerService.Application.Features.Customers.Queries.GetCustomersByUserIds;

public record GetCustomersByUserIdsResult(List<Guid> UserIds) : IQuery<List<CustomerDto>>;