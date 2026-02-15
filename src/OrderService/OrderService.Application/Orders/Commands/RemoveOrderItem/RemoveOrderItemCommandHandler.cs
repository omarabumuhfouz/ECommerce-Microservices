using OrderService.Application.Orders.Specifications;
using OrderService.Domain.Errors;
using OrderService.Domain.Orders;
using OrderService.Domain.Orders.ValueObjects;
using SharedKernel.Abstractions;
using SharedKernel.Common;

namespace OrderService.Application.Orders.Commands.RemoveOrderItem;

public class RemoveOrderItemCommandHandler : ICommandHandler<RemoveOrderItemCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveOrderItemCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(RemoveOrderItemCommand request, CancellationToken ct)
    {
        return await _unitOfWork.GetRepository<Order>()
            .FirstOrDefaultAsync(new GetOrderByIdSpec(request.OrderId, true), ct)
            .ToResult(DomainErrors.Order.NotFound(request.OrderId))
            .Bind(order => order.RemoveOrderItem(request.ItemId))
            .Tap(async _ => await _unitOfWork.SaveChangesAsync(ct))
            .Map(_ => Unit.Value);
    }
}