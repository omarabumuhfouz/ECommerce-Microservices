using SharedKernel.Primitives;

namespace OrderService.Domain.Orders.Events;

/// <summary>
/// Event raised when an existing order item is updated in a Pending order.
/// </summary>
/// <param name="OrderId">The unique identifier of the order.</param>
/// <param name="ProductId">The product identifier.</param>
/// <param name="NewQuantity">The total quantity after the update.</param>
/// <param name="QuantityDelta">
/// The difference between the new and old quantity. 
/// Positive means items were added, negative means items were removed.
/// </param>
/// <param name="NewUnitPrice">The updated price of the item.</param>
public record ItemUpdatedDomainEvent(
    Guid OrderId,
    Guid ProductId,
    int NewQuantity,
    int QuantityDelta) : IDomainEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}