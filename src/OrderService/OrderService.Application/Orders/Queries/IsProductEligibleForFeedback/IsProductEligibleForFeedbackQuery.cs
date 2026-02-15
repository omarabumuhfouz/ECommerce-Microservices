using SharedKernel.Abstractions;

namespace OrderService.Application.Orders.Queries.IsProductEligibleForFeedback;

public record IsProductEligibleForFeedbackQuery(Guid CustomerId, Guid ProductId) : IQuery<bool>;