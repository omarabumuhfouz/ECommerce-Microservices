using ProductService.Domain.Constants;
using ProductService.Domain.Errors;
using SharedKernel.Shared;

namespace ProductService.Domain.ValueObjects
{
    public record Money
    {
        private Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency.ToUpperInvariant();
        }

        private Money() { }

        public decimal Amount { get; init; }
        public string Currency { get; init; }

        public static Result<Money> Create(decimal amount, string currency = "USD")
        {
            if (amount < ProductConstants.PRICE_MIN) return DomainErrors.Money.InvalidAmount;

            if (string.IsNullOrWhiteSpace(currency)) return DomainErrors.Money.CurrencyRequired;

            return new Money(amount, currency);
        }

        public static Money Zero()
        {
            return new Money(0, "USD");
        }
    }

}
