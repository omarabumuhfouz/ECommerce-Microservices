using System;
using FluentAssertions;
using Xunit;
using ProductService.Domain.ValueObjects;

namespace ProductService.Domain.Tests.ValueObjects;

public class FeatureTests
{
    [Fact]
    public void Create_WithValidInputs_ShouldCreateFeature()
    {
        // Arrange
        var name = "Color";
        var value = "Red";

        // Act
        var feature = Feature.Create(name, value);

        // Assert
        feature.Should().NotBeNull();
        feature.Name.Should().Be(name);
        feature.Value.Should().Be(value);
    }

    [Theory]
    [InlineData(null, "ValidValue", "name", "Feature Name must be provided.")]
    [InlineData("", "ValidValue", "name", "Feature Name must be provided.")]
    [InlineData("   ", "ValidValue", "name", "Feature Name must be provided.")]
    [InlineData("ValidName", null, "value", "Feature value must be provided.")]
    [InlineData("ValidName", "", "value", "Feature value must be provided.")]
    [InlineData("ValidName", "   ", "value", "Feature value must be new provided.")]
    public void Create_WithInvalidInput_ShouldThrowArgumentException(
        string name, string value, string expectedParamName, string expectedMessage)
    {
        // Arrange
        Action act = () => Feature.Create(name, value);

        // Act & Assert
        // This is the updated, more complete assertion
        act.Should()
           .Throw<ArgumentException>()
           .WithMessage(expectedMessage + "*") // Use wildcard for " (Parameter '...')"
           .And.ParamName.Should().Be(expectedParamName);
    }
}