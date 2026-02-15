using OrderService.Application.Orders.Specifications;
using OrderService.Domain.Errors;
using OrderService.Domain.Orders;
using SharedKernel.Abstractions;
using SharedKernel.Common;

namespace OrderService.Application.Orders.Commands.ExpireOrder;

public class ExpireOrderCommandHandler : ICommandHandler<ExpireOrderCommand, Unit>
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ExpireOrderCommandHandler( IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = unitOfWork.GetRepository<Order>();
    }

    public async Task<Result<Unit>> Handle(ExpireOrderCommand request, CancellationToken ct)
    {
        return  await _orderRepository
            .FirstOrDefaultAsync(new GetOrderByIdSpec(request.OrderId, true), ct)
            .ToResult(DomainErrors.Order.NotFound(request.OrderId))
            .Tap(order => order.SetStatusToCanceled())
            .Tap(async _ => await _unitOfWork.SaveChangesAsync(ct))
            .Map(_ => Unit.Value);
    }
}