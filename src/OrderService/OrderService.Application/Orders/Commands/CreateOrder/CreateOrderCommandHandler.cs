using System.Reflection.Metadata;
using Microsoft.AspNetCore.Http.Features;
using OrderService.Api.Contracts.Orders;
using OrderService.Application.Services;
using OrderService.Domain.Constants;
using OrderService.Domain.Errors;
using OrderService.Domain.Orders;
using OrderService.Domain.Orders.ValueObjects;
using Quartz;
using SharedKernel.Abstractions;

namespace OrderService.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Order> _orderRepo;
    private readonly IValidationService _validationService;
    private readonly ICustomerService _customerService;
    private readonly IProductService _productService;
    private readonly ILogger<CreateOrderCommandHandler> _logger;
private readonly IOrderExpirationScheduler _expirationScheduler;

    public CreateOrderCommandHandler(
        IUnitOfWork unitOfWork,
        IValidationService validationService,
        ICustomerService customerService,
        IProductService productService,
        ILogger<CreateOrderCommandHandler> logger,
        IOrderExpirationScheduler expirationScheduler
        )
    {
        _unitOfWork = unitOfWork;
        _orderRepo = unitOfWork.GetRepository<Order>();
        _validationService = validationService;
        _customerService = customerService;
        _productService = productService;
        _logger = logger;
        _expirationScheduler = expirationScheduler;
    }

    async Task<Result<Guid>> IRequestHandler<CreateOrderCommand, Result<Guid>>.Handle(CreateOrderCommand request, CancellationToken ct)
    {
        var orderId = Guid.NewGuid();
        return await PrepareOrderItemsAsync(orderId, request.OrderItems)
        .Bind(async items => await GetCreateOrderContextAsync(request).Map(context => (Items:items, Context:context)))
        .Map(data => CreateOrderAggregate(orderId, request, data.Items, data.Context.Customer, data.Context.Billing, data.Context.Shipping))
        .Bind(async order => await AddItemsToOrderAsync(order, request.OrderItems))
        .Tap(async order => await _orderRepo.AddAsync(order, ct))
        .Tap(async _ => await _unitOfWork.SaveChangesAsync(ct))
        .Tap(async order => await _expirationScheduler.ScheduleExpirationAsync(order.Id, ct))
        .Map(order => order.Id);
    }

    private async Task<Result<(CustomerDto Customer, AddressDto Billing, AddressDto Shipping)>> GetCreateOrderContextAsync(CreateOrderCommand command)
    {
        return await _customerService.GetCustomerByIdAsync(command.CustomerId)
        .Bind(customer => customer.GetAddressById(command.BillingAddressId)
            .Map(billing => (Customer: customer, Billing: billing)))
        .Bind(state =>
            state.Customer.GetAddressById(command.ShippingAddressId)
                .Map(shipping => (
                    state.Customer,
                    state.Billing,
                    Shipping: shipping
                    ))
        );
    }



    private async Task<Result<List<OrderItem>>> PrepareOrderItemsAsync(Guid orderId, List<CreateItemDto> items)
    {
        var productIds = items.Select(i => i.ProductId).Distinct().ToList();

        var productsResult = await _productService.GetProductsByIdsAsync(productIds);
        if (productsResult.IsFailure)
        {
            _logger.LogWarning("Batch product lookup failed: {ErrorCode}", productsResult.TopError.Code);
            return productsResult.TopError;
        }

        var productMap = productsResult.Value.ToDictionary(p => p.Id);
        var orderItems = new List<OrderItem>(items.Count);

        foreach (var item in items)
        {
            if (!productMap.TryGetValue(item.ProductId, out var product))
            {
                _logger.LogWarning("Product {ProductId} not found in database", item.ProductId);
                return DomainErrors.Product.NotFound(item.ProductId);
            }

            var stockResult = _validationService.ValidateStock(product, item.Quantity);
            if (stockResult.IsFailure)
            {
                _logger.LogWarning("Insufficient stock for Product {ProductId}", item.ProductId);
                return stockResult.TopError;
            }

            var orderItemResult = OrderItem.Create(
                orderId,
                item.ProductId,
                item.ProductName,
                item.Quantity,
                product.Price,
                product.DiscountPercentage
            );

            if (orderItemResult.IsFailure)
            {
                return orderItemResult.TopError;
            }

            orderItems.Add(orderItemResult.Value);
        }

        return orderItems;
    }

    // ... Helper methods (GenerateOrderNumber, RandomNumber) remain the same ...
    private string GenerateOrderNumber()
    {
        return $"ORD-{DateTime.UtcNow.ToString("yyyyMMdd-HHmmss")}-{RandomNumber(1000, 9999)}";
    }

    private int RandomNumber(int min, int max)
    {
        using (var rng = RandomNumberGenerator.Create())
        {
            var bytes = new byte[4];
            rng.GetBytes(bytes);
            return Math.Abs(BitConverter.ToInt32(bytes, 0) % (max - min + 1)) + min;
        }
    }

    private Order CreateOrderAggregate(
      Guid id,
      CreateOrderCommand req,
      List<OrderItem> items,
      CustomerDto Customer, AddressDto Billing, AddressDto Shipping)
    {
        // We call the Domain Factory (Order.Create). 
        // Since validation happened in earlier ROP steps, we access .Value directly.
        return Order.Create(
            id,
            req.CustomerId,
            GenerateOrderNumber(),
            req.BillingAddressId,
            req.ShippingAddressId,
            OrderConstants.ShippingCost,
            items
        ).Value;
    }

    private async Task<Result<Order>> AddItemsToOrderAsync(Order order, List<CreateItemDto> itemsDto)
    {
        // Re-prepare or validate items if necessary, then add to aggregate
        var itemsResult = await PrepareOrderItemsAsync(order.Id, itemsDto);

        return itemsResult.Map(items =>
        {
            order.AddItems(items);
            return order;
        });
    }

}
