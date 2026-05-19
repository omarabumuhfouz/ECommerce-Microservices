using InventoryService.Domain.ValueObjects;
using SharedKernel.Primitives;

namespace InventoryService.Domain.InventoryItems;

public sealed class StockReservation : Entity
{
    public Guid OrderId { get; private set; }
    public Quantity Quantity { get; private set; }
    public DateTime ReservedAtUtc { get; private set; }

    internal StockReservation(Guid id, Guid orderId, Quantity quantity, DateTime reservedAtUtc)
        : base(id)
    {
        OrderId = orderId;
        Quantity = quantity;
        ReservedAtUtc = reservedAtUtc;
    }

    internal void UpdateQuantity(Quantity newQuantity)
    {
        Quantity = newQuantity;
    }

    #pragma warning disable CS8618 
    private StockReservation() { }
    #pragma warning restore CS8618
}