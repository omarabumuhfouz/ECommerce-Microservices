using MediatR;
using SharedKernel.Primitives;

namespace SharedKernel.Abstractions.Messaging;

public interface IDomainEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : IDomainEvent
{
}
