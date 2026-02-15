using OrderService.Api.Contracts.Orders;
using OrderService.Application.Orders.Specifications;
using OrderService.Application.Services;
using OrderService.Domain.Errors;
using OrderService.Domain.Orders;
using SharedKernel.Abstractions;
using SharedKernel.Common;

namespace OrderService.Application.Orders.Commands.UpdateOrderItems;

public class UpdateOrderItemsCommandHandler : ICommandHandler<UpdateOrderItemsCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Order> _orderRepo;
    private readonly IProductService _productService;
    private readonly IValidationService _validationService;
    private readonly ILogger<UpdateOrderItemsCommandHandler> _logger;

    public UpdateOrderItemsCommandHandler(IUnitOfWork unitOfWork, IProductService productService, IValidationService validationService, ILogger<UpdateOrderItemsCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _orderRepo = unitOfWork.GetRepository<Order>();
        _productService = productService;
        _validationService = validationService;
        _logger = logger;
    }

    public async Task<Result<Unit>> Handle(UpdateOrderItemsCommand request, CancellationToken ct)
    {
        return await _orderRepo.FirstOrDefaultAsync(new GetOrderByIdSpec(request.OrderId, true), ct)
        .ToResult(DomainErrors.Order.NotFound(request.OrderId))
        .Bind(async order => await UpdateOrderItems(order, request.OrderItems))
        .Tap(async _ => await _unitOfWork.SaveChangesAsync(ct))
        .Map(_ => Unit.Value);

    }

    private async Task<Result<Unit>> UpdateOrderItems(Order order, List<UpdateItemsDto> requestedItems)
    {
        var orderItemsMap = order.OrderItems.ToDictionary(i => i.Id);
        var itemsToUpdate = new List<(OrderItem OrderItem, int Quantity)>();
        var productIds = new List<Guid>();

        foreach (var reqItem in requestedItems)
        {
            if (!orderItemsMap.TryGetValue(reqItem.ItemId, out var orderItem))
            {
                return DomainErrors.Order.ItemNotFound;
            }
            itemsToUpdate.Add((orderItem, reqItem.Quantity));
            productIds.Add(orderItem.ProductId);
        }

        // 3. Fetch product details (Price, Stock) for validation and update
        var productsResult = await _productService.GetProductsByIdsAsync(productIds.Distinct().ToList());
        if (productsResult.IsFailure) return productsResult.TopError;

        var productsMap = productsResult.Value.ToDictionary(p => p.Id);

        // 4. Validate Stock and Update Items
        foreach (var (orderItem, quantity) in itemsToUpdate)
        {
            if (!productsMap.TryGetValue(orderItem.ProductId, out var product))
                return DomainErrors.Product.NotFound(orderItem.ProductId);

            var stockResult = _validationService.ValidateStock(product, quantity);
            if (stockResult.IsFailure) return stockResult.TopError;

            var updateOrderItmeResult = order.UpdateOrderItem(
                orderItem.ProductId,
                quantity,
                product.Price,
                product.DiscountPercentage);

            if (updateOrderItmeResult.IsFailure) return updateOrderItmeResult.TopError;
        }

        return Unit.Value;
    }
}