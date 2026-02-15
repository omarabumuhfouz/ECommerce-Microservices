using OrderService.Domain.Orders;
using SharedKernel.Abstractions;

namespace OrderService.Application.Orders.Queries.GetOrdersCountByCustomer;

public class GetOrdersCountsByCustomerIdQueryHandler : IQueryHandler<GetOrdersCountsByCustomerIdQuery, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetOrdersCountsByCustomerIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    async Task<Result<int>> IRequestHandler<GetOrdersCountsByCustomerIdQuery, Result<int>>.Handle(GetOrdersCountsByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.GetRepository<Order>()
                .CountAsync(o => o.CustomerId == request.CustomerId, cancellationToken);
    }
}