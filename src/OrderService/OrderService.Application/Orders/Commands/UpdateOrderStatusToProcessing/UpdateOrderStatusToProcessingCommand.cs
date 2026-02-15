using SharedKernel.Abstractions.Messaging;
using SharedKernel.Primitives.Result;

namespace OrderService.Application.Orders.Commands.UpdateOrderStatusToProcessing;

public sealed record UpdateOrderStatusToProcessingCommand(Guid OrderId, Guid PaymentId) : ICommand<Unit>;