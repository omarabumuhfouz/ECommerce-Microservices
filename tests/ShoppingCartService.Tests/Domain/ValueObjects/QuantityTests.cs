namespace ShoppingCartService.Tests.ValueObjectsTests;

public class QuantityTests
{

    [Fact]
    public void Create_ShouldCreateQuantity_WhenValueIsValid()
    {
        var quantity = Quantity.Create(10);

        quantity.Value.Should().Be(10);
    }


    [Fact]
    public void Create_ShouldThrow_WhenValueIsInvalid()
    {
        Action act = () => Quantity.Create(-1);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void ImplicitConversion_ToInt_ShouldReturnIntValue()
    {
        Quantity quantity = Quantity.Create(42);

        int value = quantity;

        value.Should().Be(42);
    }

    [Fact]
    public void ExplicitConversion_FromInt_ShouldCreateQuantity()
    {
        int input = 75;

        var quantity = (Quantity)input;

        quantity.Value.Should().Be(75);
    }

    [Fact]
    public void Equality_ShouldBeTrue_ForSameValue()
    {
        var q1 = Quantity.Create(25);
        var q2 = Quantity.Create(25);

        q1.Should().Be(q2);
        (q1 == q2).Should().BeTrue();
    }

    [Fact]
    public void Equality_ShouldBeFalse_ForDifferentValues()
    {
        var q1 = Quantity.Create(25);
        var q2 = Quantity.Create(26);

        q1.Should().NotBe(q2);
        (q1 != q2).Should().BeTrue();
    }
}
