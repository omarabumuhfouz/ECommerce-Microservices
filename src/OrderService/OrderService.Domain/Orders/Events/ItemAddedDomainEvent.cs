using SharedKernel.Primitives;

namespace OrderService.Domain.Orders.Events
{
    public record ItemAddedDomainEvent(
        Guid OrderId,
        Guid ProductId,
        int Quantity,
        decimal UnitPrice
    ) : IDomainEvent
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    }

}