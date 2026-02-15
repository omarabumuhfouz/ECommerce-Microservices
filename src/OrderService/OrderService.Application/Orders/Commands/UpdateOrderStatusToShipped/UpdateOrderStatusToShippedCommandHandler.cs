using OrderService.Application.Orders.Specifications;
using OrderService.Domain.Errors;
using OrderService.Domain.Orders;
using SharedKernel.Abstractions;
using SharedKernel.Common;

namespace OrderService.Application.Orders.Commands.UpdateOrderStatusToShipped;

public class UpdateOrderStatusToShippedCommandHandler : ICommandHandler<UpdateOrderStatusToShippedCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Order> _orderRepo;

    public UpdateOrderStatusToShippedCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _orderRepo = unitOfWork.GetRepository<Order>();
    }

    public async Task<Result<Unit>> Handle(UpdateOrderStatusToShippedCommand request, CancellationToken ct)
    {
        return await _orderRepo.FirstOrDefaultAsync(new GetOrderByIdSpec(request.OrderId, true), ct)
            .ToResult(DomainErrors.Order.NotFound(request.OrderId))
            .Bind(order => order.SetStatusToShipped())
            .Tap(async _ => await _unitOfWork.SaveChangesAsync(ct))
            .Map(_ => Unit.Value);
    }
}