using SharedKernel.Primitives;

namespace PaymentService.Domain.Payments.Events;

public record PaymentFailedDomainEvent(
    Guid PaymentId,
    Guid OrderId,
    decimal Amount
) : IDomainEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
