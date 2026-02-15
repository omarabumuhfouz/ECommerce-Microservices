using CancellationService.Domain.Cancellations.Enums;
using SharedKernel.Common;

namespace CancellationService.Application.Cancellations.Queries.GetCancellations;

public record GetCancellationsQuery(
    int PageNumber,
    int PageSize,
    string? SearchTerm,
    CancellationStatus? Status, // Enum is fine here
    string? SortBy,
    bool IsAscending
) : IQuery<PagedList<CancellationDto>>;
