namespace ShoppingCartService.Tests.ValueObjectsTests;

public class MoneyTests
{

    [Fact]
    public void Create_ShouldCreateMoney_WhenValueIsInRange()
    {
        var money = Money.Create(100);
        money.Value.Should().Be(decimal.Round(100, 2));
    }

    [Fact]
    public void Create_ShouldThrow_WhenValueIsOutOfRange()
    {
        Action act = () => Money.Create(-100);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Operator_Add_ShouldReturnCorrectSum()
    {
        var m1 = Money.Create(150.25m);
        var m2 = Money.Create(49.75m);

        var result = m1 + m2;

        result.Value.Should().Be(200.00m);
    }

    [Fact]
    public void Operator_Subtract_ShouldReturnCorrectDifference()
    {
        var m1 = Money.Create(200.00m);
        var m2 = Money.Create(50.00m);

        var result = m1 - m2;

        result.Value.Should().Be(150.00m);
    }

    [Fact]
    public void Operator_Subtract_ShouldAllowZeroResult()
    {
        var m1 = Money.Create(100.00m);
        var m2 = Money.Create(100.00m);

        var result = m1 - m2;

        result.Value.Should().Be(0.00m);
    }

    [Fact]
    public void ImplicitConversion_ToDecimal_ShouldReturnDecimalValue()
    {
        Money money = Money.Create(123.45m);
        decimal value = money;

        value.Should().Be(123.45m);
    }

    [Fact]
    public void ExplicitConversion_FromDecimal_ShouldCreateMoney()
    {
        decimal input = 456.78m;
        var money = (Money)input;

        money.Value.Should().Be(456.78m);
    }

    [Fact]
    public void Create_ShouldRoundValueToTwoDecimals()
    {
        var money = Money.Create(123.4567m);
        money.Value.Should().Be(123.46m);
    }

    [Fact]
    public void Equality_ShouldBeTrue_ForSameValue()
    {
        var m1 = Money.Create(99.99m);
        var m2 = Money.Create(99.99m);

        m1.Should().Be(m2);
        (m1 == m2).Should().BeTrue();
    }

    [Fact]
    public void Equality_ShouldBeFalse_ForDifferentValue()
    {
        var m1 = Money.Create(99.99m);
        var m2 = Money.Create(100.00m);

        m1.Should().NotBe(m2);
        (m1 != m2).Should().BeTrue();
    }
}
