
namespace CancellationService.Api.Contracts.Cancellations;

public record GetCancellationsRequest(
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    string? Status = null, // String instead of Enum
    string? SortBy = "RequestedAt",
    bool IsAscending = false
);