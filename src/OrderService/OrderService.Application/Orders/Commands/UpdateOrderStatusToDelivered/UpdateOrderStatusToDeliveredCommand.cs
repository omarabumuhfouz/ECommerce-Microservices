using SharedKernel.Abstractions.Messaging;
using SharedKernel.Primitives.Result;

namespace OrderService.Application.Orders.Commands.UpdateOrderStatusToDelivered;

public sealed record UpdateOrderStatusToDeliveredCommand(Guid OrderId, Guid PaymentId) : ICommand<Unit>;