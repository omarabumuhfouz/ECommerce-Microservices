namespace ProductService.Tests.Domain.ValueObjects;

public class DiscountTests
{
    [Fact]
    public void Create_ShouldSucceed_WithValidPercentage()
    {
        var discount = Discount.Create(25);
        discount.Percentage.Should().Be(25);
        discount.EndDate.Should().BeNull();
    }

    [Fact]
    public void Create_ShouldThrow_WhenPercentageOutOfRange()
    {
        Action act = () => Discount.Create(110);
        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*between 0 and 100*");
    }

    [Fact]
    public void Create_ShouldThrow_WhenEndDateIsInThePast()
    {
        var pastDate = DateTime.UtcNow.AddDays(-1);
        Action act = () => Discount.Create(10, pastDate);
        act.Should().Throw<ArgumentException>().WithMessage("*in the past*");
    }

    [Fact]
    public void Create_ShouldSucceed_WithFutureEndDate()
    {
        var futureDate = DateTime.UtcNow.AddDays(10);
        var discount = Discount.Create(15, futureDate);
        discount.EndDate.Should().Be(futureDate);
    }
}

