using OrderService.Application.Orders.Specifications;
using OrderService.Domain.Orders;
using SharedKernel.Abstractions;
using SharedKernel.Abstractions.Data;
using SharedKernel.Common;

namespace OrderService.Application.Orders.Queries.GetOrders;

public class GetOrdersQueryHandler : IQueryHandler<GetOrdersQuery, PagedList<OrderDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Order> _orderRepo;
    private readonly IMapper _mapper;
    private readonly ILogger<GetOrdersQueryHandler> _logger;

    public GetOrdersQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetOrdersQueryHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _orderRepo = _unitOfWork.GetRepository<Order>();
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<PagedList<OrderDto>>> Handle(GetOrdersQuery request, CancellationToken ct)
{
    var spec = new GetOrdersSpec(
        request.SearchTerm, 
        request.Status, 
        request.CustomerId, 
        request.SortBy, 
        request.IsAscending, 
        request.PageNumber, 
        request.PageSize);

    var orders = await _orderRepo.GetListAsync(spec, ct);

    var countSpec = new OrderCountSpec(request.SearchTerm, request.Status, request.CustomerId);
    var totalCount = await _orderRepo.CountAsync(countSpec, ct);

    return new PagedList<OrderDto>(
        orders.ToList(),
        request.PageNumber,
        request.PageSize,
        totalCount
    );
}
}
