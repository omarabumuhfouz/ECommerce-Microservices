using OrderService.Domain.Orders.Enums;
using OrderService.Domain.Orders.ValueObjects;
using MediatR;
using OrderService.Domain.Constants;
using SharedKernel.Common;
using OrderService.Domain.Orders.Events;
using SharedKernel.Primitives.Results;

namespace OrderService.Domain.Orders;

public class Order : AggregateRoot,IAuditableEntity
{
    private Order(
        Guid id,
        Guid customerId,
        OrderNumber orderNumber,
        DateTime orderDate,
        Guid billingAddressId,
        Guid shippingAddressId,
        Money shippingCost,
        List<OrderItem>? orderItems = null) : base(id)
    {
        CustomerId = customerId;
        OrderNumber = orderNumber;
        OrderDate = orderDate;
        BillingAddressId = billingAddressId;
        ShippingAddressId = shippingAddressId;
        ShippingCost = shippingCost;

        // Defaults
        OrderStatus = OrderStatus.Pending;
        _orderItems = orderItems ?? new List<OrderItem>();

        RaiseDomainEvent(new OrderCreatedDomainEvent(
            Id,
            CustomerId,
            _orderItems.Select(oi => new OrderEventItem(oi.ProductId, oi.Quantity)).ToList()
        ));
    }

    private Order() { }


    public Guid CustomerId { get; private set; }
    public Guid? PaymentId { get; private set; } = null;
    public OrderNumber OrderNumber { get; private set; }
    public DateTime OrderDate { get; private set; }
    public Guid BillingAddressId { get; private set; }
    public Guid ShippingAddressId { get; private set; }

    public OrderStatus OrderStatus { get; private set; }

    public Money ShippingCost { get; private set; }

    public Money TotalBaseAmount => _orderItems
        .Aggregate(Money.Zero, (sum, item) => sum + (item.UnitPrice * item.Quantity));

    public Money TotalDiscountAmount => _orderItems
        .Aggregate(Money.Zero, (sum, item) => sum + item.Discount);

    public Money TotalAmount => TotalBaseAmount - TotalDiscountAmount + ShippingCost;

    private readonly List<OrderItem> _orderItems;
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public DateTime CreatedOnUtc { get ; set ; }
    public DateTime? ModifiedOnUtc { get ; set ; }

    public static Result<Order> Create(
        Guid orderId,
        Guid customerId,
        string orderNumber,
        Guid billingAddressId,
        Guid shippingAddressId,
        decimal shippingCost,
        List<OrderItem>? orderItems = null)
    {
        var orderNumberResult = OrderNumber.Create(orderNumber);
        var shippingMoneyResult = Money.Create(shippingCost);

        if (orderNumberResult.IsFailure) return orderNumberResult.TopError;
        if (shippingMoneyResult.IsFailure) return shippingMoneyResult.TopError;

        return new Order(
            orderId,
            customerId,
            orderNumberResult.Value,
            DateTime.UtcNow,
            billingAddressId,
            shippingAddressId,
            shippingMoneyResult.Value,
            orderItems

        );
    }

    public Result<Unit> TransitionTo(OrderStatus newStatus)
    {
        var validationResult = OrderStateMachine.EnsureValidTransition(OrderStatus, newStatus);

        if (validationResult.IsFailure) return validationResult.TopError;

        OrderStatus = newStatus;
        
        RaiseDomainEventForStatus(newStatus);


        return Unit.Value;
    }

    public Result<Unit> SetStatusToProcessing() => TransitionTo(OrderStatus.Processing);
    public Result<Unit> SetStatusToShipped() => TransitionTo(OrderStatus.Shipped);
    public Result<Unit> SetStatusToDelivered() => TransitionTo(OrderStatus.Delivered);
    public Result<Unit> SetStatusToRefunded() => TransitionTo(OrderStatus.Refunded);
    public Result<Unit> SetStatusToCanceled() => TransitionTo(OrderStatus.Canceled);
    
    private void RaiseDomainEventForStatus(OrderStatus newStatus)
    {
        var domainEvent = newStatus switch
        {
            OrderStatus.Processing => new OrderProcessingDomainEvent(this.Id, this.CustomerId, this._orderItems.Select(oi => new OrderEventItem(oi.ProductId, oi.Quantity)).ToList()),
            OrderStatus.Shipped => new OrderShippedDomainEvent(this.Id, this.CustomerId, this._orderItems.Select(oi => new OrderEventItem(oi.ProductId, oi.Quantity)).ToList()),
            OrderStatus.Delivered => new OrderDeliveredDomainEvent(this.Id, this.CustomerId, this._orderItems.Select(oi => new OrderEventItem(oi.ProductId, oi.Quantity)).ToList()),
            OrderStatus.Refunded => new OrderRefundedDomainEvent(this.Id, this.CustomerId, this._orderItems.Select(oi => new OrderEventItem(oi.ProductId, oi.Quantity)).ToList()),
            OrderStatus.Canceled => new OrderCanceledDomainEvent(this.Id, this.CustomerId, this._orderItems.Select(oi => new OrderEventItem(oi.ProductId, oi.Quantity)).ToList()),
            _ => (IDomainEvent?)null
        };

        if (domainEvent is not null)
        {
            this.RaiseDomainEvent(domainEvent);
        }
    } 

    public Result<Unit> AddOrderItem(OrderItem item)
    {
        return EnsureIsPending()
        .Ensure(_ => item is not null, Error.NullValue)
        .Ensure(_ => !_orderItems
             .Any(oi => oi.ProductId == item.ProductId),
                    DomainErrors.Order.DuplicateProduct)
        .Tap(_ => _orderItems.Add(item))
        .Tap(_ => this.RaiseDomainEvent(new ItemAddedDomainEvent(this.Id, item.ProductId, item.Quantity, item.UnitPrice)))
        .Map(_ => Unit.Value);
    }


    public Result AddItems(IEnumerable<OrderItem> items)
    {
        var ensurePendingResult = EnsureIsPending();
        if (ensurePendingResult.IsFailure) return ensurePendingResult.TopError;

        foreach (var item in items)
        {
            var addResult = AddOrderItem(item);
            if (addResult.IsFailure) return addResult.TopError;
        }
        return Result.Success();
    }

    public Result<OrderItem> GetItemById(Guid itemId)
    {
        var orderItem = _orderItems.FirstOrDefault(item => item.Id == itemId);
        if (orderItem is null) DomainErrors.Items.NotFound(itemId);

        return orderItem;
    }

    public Result<Unit> UpdateOrderItem(Guid itemId, int newQuantity, decimal newUnitPrice, decimal newDiscount)
    {
        var item = _orderItems.FirstOrDefault(oi => oi.Id == itemId);
    if (item is null) return DomainErrors.Order.ItemNotFound;

    var result = EnsureIsPending();
    if (result.IsFailure) return result.TopError;

    int oldQuantity = item.Quantity;
    int quantityDelta = newQuantity - oldQuantity;

    return item.Update(newQuantity, newUnitPrice, newDiscount)
        .Tap(_ => {
            this.RaiseDomainEvent(new ItemUpdatedDomainEvent(
                this.Id, 
                item.ProductId, 
                newQuantity, 
                quantityDelta)); 
        })
        .Map(_ => Unit.Value);
    }

    public Result<Unit> RemoveOrderItem(Guid itemId)
    {
        return EnsureIsPending()
             .Ensure(_ => _orderItems.Count > OrderConstants.MinimumRequiredItems,
                 DomainErrors.Order.CannotRemoveLastItem)
            .Bind(_ => _orderItems
                     .FirstOrDefault(oi => oi.Id == itemId)
                     .ToResult(DomainErrors.Order.ItemNotFound))
            .Tap(item => _orderItems.Remove(item)) 
            .Tap(item => this.RaiseDomainEvent(new ItemRemovedDomainEvent(this.Id, item.ProductId, item.Quantity)))
            .Map(_ => Unit.Value);
    }



    private Result<Unit> EnsureIsPending()
    {
        if (OrderStatus is OrderStatus.Pending) return Unit.Value;

        return DomainErrors.Order.CannotModifyNonPendingOrder(Id, OrderStatus.ToString());
    }

public Result<Unit> LinkPayment(Guid paymentId)
{
    // Business Rule: Don't allow changing the payment ID if it's already set 
    // unless it's a specific re-payment scenario.
    if (PaymentId.HasValue && PaymentId.Value != paymentId)
    {
        return DomainErrors.Order.PaymentAlreadyLinked(this.Id);
    }

    PaymentId = paymentId;
    return Unit.Value;
}
}
