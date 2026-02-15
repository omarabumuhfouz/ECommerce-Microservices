using OrderService.Application.Orders.Specifications;
using OrderService.Application.Services;
using OrderService.Domain.Errors;
using OrderService.Domain.Orders;
using OrderService.Domain.Orders.ValueObjects;
using SharedKernel.Abstractions;
using SharedKernel.Common;

namespace OrderService.Application.Orders.Commands.AddOrderItem;

public class AddOrderItemCommandHandler : ICommandHandler<AddOrderItemCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Order> _orderRepo;
    private readonly IProductService _productService;
    private readonly ILogger<AddOrderItemCommandHandler> _logger;

    public AddOrderItemCommandHandler(
        IUnitOfWork unitOfWork,
        IProductService productService,
        ILogger<AddOrderItemCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _orderRepo = unitOfWork.GetRepository<Order>();
        _productService = productService;
        _logger = logger;
    }


    public async Task<Result<Unit>> Handle(AddOrderItemCommand request, CancellationToken ct)
    {
        return  await _orderRepo.FirstOrDefaultAsync(new GetOrderByIdSpec(request.OrderId, true), ct)
                           .ToResult(DomainErrors.Order.NotFound(request.OrderId))
                           .Bind(async order => await _productService.GetProductByIdAsync(request.ProductId)
                                    .Map(product => (order, product)))
                            .Bind(data => OrderItem.Create(
                                        data.order.Id,
                                        data.product.Id,
                                        request.ProductName,
                                        request.Quantity,
                                        data.product.Price,
                                        data.product.DiscountPercentage
                                 ).Map(item => (data.order, item)))
                            .Bind(data => data.order.AddOrderItem(data.item))
                            .Tap(async _ => await _unitOfWork.SaveChangesAsync(ct))
                            .Map(_ => Unit.Value);
    }
}