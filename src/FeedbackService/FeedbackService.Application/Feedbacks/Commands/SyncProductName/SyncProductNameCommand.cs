namespace FeedbackService.Application.Feedbacks.Commands.SyncProductName;

public record SyncProductNameCommand(Guid ProductId, string NewName) : ICommand<Unit>;