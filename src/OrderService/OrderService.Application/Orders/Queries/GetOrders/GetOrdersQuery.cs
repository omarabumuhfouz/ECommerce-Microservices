using SharedKernel.Common;

namespace OrderService.Application.Orders.Queries.GetOrders;

public record GetOrdersQuery(
    string? SearchTerm = null,
    string? Status = null,
    Guid? CustomerId = null,
    int PageNumber = 1,
    int PageSize = 10,
    string SortBy = "Date",
    bool IsAscending = false // Usually best to show newest first
) : IQuery<PagedList<OrderDto>>;
