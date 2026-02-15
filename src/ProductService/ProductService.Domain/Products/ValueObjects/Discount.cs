using ProductService.Domain.Constants;
using ProductService.Domain.Errors;
using SharedKernel.Shared;

namespace ProductService.Domain.ValueObjects;

public record Discount
{
    private Discount(int percentage, DateTime? endDate)
    {
        Percentage = percentage;
        EndDate = endDate;
    }

    private Discount() { } 

    public int Percentage { get; init; }    
    public DateTime? EndDate { get; init; } 

    public static Result<Discount> Create(int percentage, DateTime? endDate = null)
    {
        if (percentage < ProductConstants.DISCOUNT_MIN || percentage > ProductConstants.DISCOUNT_MAX)
            return DomainErrors.Discount.InvalidPercentage;


        if (endDate.HasValue && endDate.Value < DateTime.UtcNow) return DomainErrors.Discount.InvalidEndDate;

        return new Discount(percentage, endDate);
    }
    public static Discount Zero()
    {
        return new Discount(0, null);
    }
}
