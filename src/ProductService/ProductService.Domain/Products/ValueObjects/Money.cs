using ProductService.Domain.Constants;
using ProductService.Domain.Errors;
using SharedKernel.Primitives.Results;

namespace ProductService.Domain.ValueObjects
{
    public record Money
    {
        private Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency.ToUpperInvariant();
        }

        #pragma warning disable CS8618 
    private Money() { }
    #pragma warning restore CS8618

        public decimal Amount { get; init; }
        public string Currency { get; init; }

        public static Result<Money> Create(decimal amount, string currency = "USD")
        {
            if (amount < ProductConstants.PriceMin) return DomainErrors.Money.InvalidAmount;

            if (string.IsNullOrWhiteSpace(currency)) return DomainErrors.Money.CurrencyRequired;

            return new Money(amount, currency);
        }

        public static Money Zero()
        {
            return new Money(0, "USD");
        }
    }

}
