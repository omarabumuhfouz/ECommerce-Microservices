namespace FeedbackService.Application.Feedbacks.Commands.SyncCustomerName;
public record SyncCustomerNameCommand(Guid CustomerId, string NewName) : ICommand<Unit>;
