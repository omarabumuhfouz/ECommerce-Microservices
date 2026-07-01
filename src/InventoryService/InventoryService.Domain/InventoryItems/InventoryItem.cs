using InventoryService.Domain.Errors;
using InventoryService.Domain.InventoryItems.Events;
using InventoryService.Domain.ValueObjects;
using SharedKernel.Primitives;
using SharedKernel.Primitives.Results;

namespace InventoryService.Domain.InventoryItems;

public sealed class InventoryItem : AggregateRoot, IAuditableEntity
{
    public Guid ProductId { get; private set; }
    
    public Quantity AvailableStock { get; private set; }
    
    public int LowStockThreshold { get; private set; }

    public int CurrentAvailableStock => AvailableStock.Value;

    public int StockOnHand => CurrentAvailableStock + ReservedStock;

    public int ReservedStock => _reservations.Sum(r => r.Quantity.Value);

    public bool IsInStock => CurrentAvailableStock > 0;

    public int Deficit => LowStockThreshold - CurrentAvailableStock;


    private readonly List<StockReservation> _reservations = new();
    public IReadOnlyCollection<StockReservation> Reservations => _reservations.AsReadOnly();

    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }

    private InventoryItem(Guid id, Guid productId, Quantity initialQuantity, int lowStockThreshold) 
        : base(id)
    {
        ProductId = productId;
        AvailableStock = initialQuantity;
        LowStockThreshold = lowStockThreshold;
    }

    #pragma warning disable CS8618 
    private InventoryItem() { }
    #pragma warning restore CS8618

    public static Result<InventoryItem> Create(Guid productId, int initialStock, int lowStockThreshold = 5)
    {
        if (productId == Guid.Empty) return DomainErrors.Inventory.ProductIdRequired;

        var quantityResult = Quantity.Create(initialStock);
        if (quantityResult.IsFailure) return quantityResult.TopError;

        var item = new InventoryItem(Guid.NewGuid(), productId, quantityResult.Value, lowStockThreshold);
        
        item.RaiseDomainEvent(new InventoryCreatedDomainEvent(item.Id, productId, initialStock));
        return item;
    }

    /// <summary>
    /// Checks if the requested quantity can be fulfilled by the current Available Stock.
    /// </summary>
    public bool CanFulfill(int requestedQuantity)
    {
        if (requestedQuantity <= 0) return false;
        return AvailableStock.Value >= requestedQuantity;
    }

    #region Stock Operations

    public Result ReceiveStock(int amount)
    {
        if (amount <= 0) return DomainErrors.Inventory.NegativeQuantity;

        var newStockResult = AvailableStock.Add(amount);
        if (newStockResult.IsFailure) return newStockResult.TopError;

        AvailableStock = newStockResult.Value;
        
        RaiseDomainEvent(new StockReceivedDomainEvent(Id, ProductId, amount));
        
        return Result.Success();
    }

    public Result ReserveStock(Guid orderId, int amount)
    {
        if (amount <= 0) return DomainErrors.Inventory.NegativeQuantity;

        // Idempotency Check
        if (_reservations.Any(r => r.OrderId == orderId)) return Result.Success();

        if (AvailableStock.Value < amount) return DomainErrors.Inventory.InsufficientStock;

        // 1. Update State
        AvailableStock = AvailableStock.Subtract(amount).Value;

        // 2. Add Reservation
        var reservation = new StockReservation(Guid.NewGuid(), orderId, Quantity.Create(amount).Value, DateTime.UtcNow);
        _reservations.Add(reservation);

        RaiseDomainEvent(new StockReservedDomainEvent(Id, ProductId, orderId, amount));

        CheckStockLevelRules();

        return Result.Success();
    }

    public Result ReleaseReservedStock(Guid orderId)
    {
        var reservation = _reservations.FirstOrDefault(r => r.OrderId == orderId);
        if (reservation is null) return Result.Success(); // Idempotent

        // Return stock to Available pile
        AvailableStock = AvailableStock.Add(reservation.Quantity.Value).Value;
        _reservations.Remove(reservation);

        RaiseDomainEvent(new ReservationReleasedDomainEvent(Id, ProductId, orderId));

        return Result.Success();
    }

    public Result DispatchStock(Guid orderId)
    {
        var reservation = _reservations.FirstOrDefault(r => r.OrderId == orderId);

        if (reservation is null) 
            return Result.Failure(DomainErrors.Inventory.ReservationNotFound(orderId));

        // Remove from reservation (It leaves the building, so it doesn't go back to AvailableStock)
        _reservations.Remove(reservation);
        
        RaiseDomainEvent(new StockDispatchedDomainEvent(Id, ProductId, orderId, reservation.Quantity.Value));

        return Result.Success();
    }

    public Result UpdateLowStockThreshold(int newThreshold)
    {
        if (newThreshold < 0) return DomainErrors.Inventory.NegativeQuantity;
        LowStockThreshold = newThreshold;
        
        CheckStockLevelRules();
        
        return Result.Success();
    }

    public Result AdjustReservation(Guid orderId, int quantityDelta)
    {
        var reservation = _reservations.FirstOrDefault(r => r.OrderId == orderId);
        if (reservation is null) return DomainErrors.Inventory.ReservationNotFound(orderId);
        if (quantityDelta == 0) return Result.Success();

        // SCENARIO A: ADDING items (Delta > 0) -> Need MORE Stock
        if (quantityDelta > 0) 
        {
            if (AvailableStock.Value < quantityDelta) return DomainErrors.Inventory.InsufficientStock;
            
            AvailableStock = AvailableStock.Subtract(quantityDelta).Value;
            reservation.UpdateQuantity(reservation.Quantity.Add(quantityDelta).Value);
        }
        // SCENARIO B: REMOVING items (Delta < 0) -> Release STOCK
        else 
        {
            var releaseAmount = Math.Abs(quantityDelta);
            AvailableStock = AvailableStock.Add(releaseAmount).Value;
            
            // Quantity.Subtract handles "cannot go below zero" validation inside
            var newQtyResult = reservation.Quantity.Subtract(releaseAmount);
            if (newQtyResult.IsFailure) return newQtyResult.TopError;

            reservation.UpdateQuantity(newQtyResult.Value);
        }

        RaiseDomainEvent(new StockReservationAdjustedDomainEvent(Id, ProductId, orderId, quantityDelta));
        
        CheckStockLevelRules();

        return Result.Success();
    }

    #endregion

    #region Private Helpers

    /// <summary>
    /// Centralized rule checking for Low Stock and Out of Stock events.
    /// </summary>
    private void CheckStockLevelRules()
    {
        if (CurrentAvailableStock == 0)
        {
            RaiseDomainEvent(new OutOfStockDomainEvent(Id, ProductId));
        }
        else if (CurrentAvailableStock <= LowStockThreshold)
        {
            RaiseDomainEvent(new LowStockThresholdReachedDomainEvent(Id, ProductId, CurrentAvailableStock));
        }
    }

    #endregion
}