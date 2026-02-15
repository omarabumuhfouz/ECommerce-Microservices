using OrderService.Application.Orders.Specifications;
using OrderService.Domain.Errors;
using OrderService.Domain.Orders;
using SharedKernel.Abstractions;
using SharedKernel.Common;

namespace OrderService.Application.Orders.Commands.UpdateOrderStatusToCanceled;

public class UpdateOrderStatusToCanceledCommandHandler : ICommandHandler<UpdateOrderStatusToCanceledCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Order> _orderRepo;

    public UpdateOrderStatusToCanceledCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _orderRepo = unitOfWork.GetRepository<Order>();
    }

    public async Task<Result<Unit>> Handle(UpdateOrderStatusToCanceledCommand request, CancellationToken ct)
    {
        return await _orderRepo.FirstOrDefaultAsync(new GetOrderByIdSpec(request.OrderId, true), ct)
            .ToResult(DomainErrors.Order.NotFound(request.OrderId))
            .Bind(order => order.SetStatusToCanceled())
            .Tap(async _ => await _unitOfWork.SaveChangesAsync(ct))
            .Map(_ => Unit.Value);
    }
}