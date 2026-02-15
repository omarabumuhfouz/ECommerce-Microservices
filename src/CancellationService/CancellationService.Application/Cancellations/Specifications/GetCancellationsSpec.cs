using System.Linq.Expressions;
using Ardalis.Specification;
using CancellationService.Domain.Cancellations.Enums;

namespace CancellationService.Application.Cancellations.Specifications;

public class GetCancellationsSpec : Specification<Cancellation>
{
    public GetCancellationsSpec(
        int pageNumber,
        int pageSize,
        string? sortBy,
        bool isAscending,
        CancellationStatus? status,
        string? searchTerm)
    {
        if (status.HasValue) Query.Where(c => c.Status == status.Value);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            Query.Where(c => c.Reason.Value.Contains(searchTerm) ||
                             (c.Remarks != null && c.Remarks.Value.Contains(searchTerm)));
        }

        ApplySorting(sortBy, isAscending);

        Query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }

    private void ApplySorting(string? sortBy, bool isAscending)
    {
        Expression<Func<Cancellation, object>> keySelector = sortBy?.ToLower() switch
        {
            "amount" => x => x.OrderAmount,
            "status" => x => x.Status,
            "processed" => x => x.ProcessedAt!,
            _ => x => x.RequestedAt // Default Sort
        };

        if (isAscending) Query.OrderBy(keySelector);
        else Query.OrderByDescending(keySelector);
    }
}