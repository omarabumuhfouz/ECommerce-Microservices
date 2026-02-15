using OrderService.Application.Orders.Specifications;
using OrderService.Domain.Orders;
using OrderService.Domain.Orders.ValueObjects;
using SharedKernel.Abstractions;

namespace OrderService.Application.Orders.Queries.GetOrdersByCustomer;

public class GetOrdersByCustomerQueryHandler : IQueryHandler<GetOrdersByCustomerQuery, List<OrderDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetOrdersByCustomerQueryHandler> _logger;


    public GetOrdersByCustomerQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetOrdersByCustomerQueryHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    async Task<Result<List<OrderDto>>> IRequestHandler<GetOrdersByCustomerQuery, Result<List<OrderDto>>>.Handle(GetOrdersByCustomerQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting orders for customer {CustomerId}", request.CustomerId);
        
        var orders = await _unitOfWork.GetRepository<Order>()
                    .GetListAsync(new GetOrdersByCustomerSpec(request.CustomerId), cancellationToken);

        _logger.LogInformation("{OrdersCount} orders for customer with Id {CustomerId} retrieved successfully", orders.Count, request.CustomerId);

        if (orders is null || !orders.Any()) return new List<OrderDto>();

        return _mapper.Map<List<OrderDto>>(orders);
    }
}
