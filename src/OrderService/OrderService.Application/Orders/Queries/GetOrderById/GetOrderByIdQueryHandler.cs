using OrderService.Application.Orders.Specifications;
using OrderService.Domain.Errors;
using OrderService.Domain.Orders;
using SharedKernel.Abstractions;
using SharedKernel.Common;

namespace OrderService.Application.Orders.Queries.GetOrderById;

public class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, OrderDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetOrderByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    async Task<Result<OrderDto>> IRequestHandler<GetOrderByIdQuery, Result<OrderDto>>.Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        return  await _unitOfWork.GetRepository<Order>()
            .FirstOrDefaultAsync(new GetOrderByIdSpec(request.OrderId), cancellationToken)
            .ToResult(DomainErrors.Order.NotFound(request.OrderId))
            .Map(order => _mapper.Map<OrderDto>(order));
    }
}
