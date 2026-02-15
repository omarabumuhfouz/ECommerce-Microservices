using OrderService.Application.Orders.Specifications;
using OrderService.Application.Services;
using OrderService.Domain.Errors;
using OrderService.Domain.Orders;
using OrderService.Domain.Orders.ValueObjects;
using SharedKernel.Abstractions;
using SharedKernel.Common;

namespace OrderService.Application.Orders.Commands.UpdateOrderItem;

public class UpdateOrderItemCommandHandler : ICommandHandler<UpdateOrderItemCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductService _productService;

    public UpdateOrderItemCommandHandler(IUnitOfWork unitOfWork, IProductService productService)
    {
        _unitOfWork = unitOfWork;
        _productService = productService;
    }

    public async Task<Result<Unit>> Handle(UpdateOrderItemCommand request, CancellationToken ct)
    {
        return await _unitOfWork.GetRepository<Order>()
        .FirstOrDefaultAsync(new GetOrderByIdSpec(request.OrderId, true), ct)
        .ToResult(DomainErrors.Order.NotFound(request.OrderId))
        .Bind(order => order.GetItemById(request.ItemId).Map(item => (order,item)))
        .Bind(data => _productService.GetProductByIdAsync(data.item.ProductId)
                    .Map(product => (data.order,product)))
        .Bind(data => data.order
                 .UpdateOrderItem(request.ItemId,
                  request.Quantity,
                  data.product.Price,
                  data.product.DiscountPercentage))
        .Tap(async _ => await _unitOfWork.SaveChangesAsync(ct))
        .Map(_ => Unit.Value);
    }
}