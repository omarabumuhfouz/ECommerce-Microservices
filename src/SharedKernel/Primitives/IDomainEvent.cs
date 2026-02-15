using MediatR;

namespace SharedKernel.Primitives;

public interface IDomainEvent : INotification
{
    public Guid Id { get; init; }
    public DateTime OccurredOn { get; init; }
}