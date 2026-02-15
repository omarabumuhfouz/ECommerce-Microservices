namespace ProductService.Tests.Domain.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void Create_ShouldSucceed_WithDefaultCurrency()
    {
        var money = Money.Create(99.99m);
        money.Amount.Should().Be(99.99m);
        money.Currency.Should().Be("USD");
    }

    [Fact]
    public void Create_ShouldSucceed_WithCustomCurrency()
    {
        var money = Money.Create(50, "eur");
        money.Currency.Should().Be("EUR");
    }

    [Theory]
    [InlineData(-1)]
    public void Create_ShouldThrow_WhenAmountNegative(decimal amount)
    {
        Action act = () => Money.Create(amount);
        act.Should().Throw<ArgumentException>().WithMessage("*Amount*");
    }

    [Fact]
    public void Create_ShouldThrow_WhenCurrencyEmpty()
    {
        Action act = () => Money.Create(10, "");
        act.Should().Throw<ArgumentException>().WithMessage("*Currency*");
    }
}

