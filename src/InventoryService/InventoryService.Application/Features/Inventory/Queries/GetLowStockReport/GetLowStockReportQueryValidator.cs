using FluentValidation;

namespace InventoryService.Application.Features.Inventory.Queries.GetLowStockReport;

/// <summary>
/// Validator for the <see cref="GetLowStockReportQuery"/>.
/// As the query has no parameters, this validator is empty.
/// </summary>
public sealed class GetLowStockReportQueryValidator : AbstractValidator<GetLowStockReportQuery>
{
    public GetLowStockReportQueryValidator()
    {
        // No validation needed for a parameter-less query.
    }
}
