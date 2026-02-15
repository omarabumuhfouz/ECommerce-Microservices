using SharedKernel.Shared;
using ShoppingCartService.Domain.Errors;

namespace ShoppingCartService.Domain.CartManagement.ValueObjects;

/// <summary>
/// Represents a validated quantity value object.
/// </summary>
public sealed record class Quantity
{
    /// <summary>
    /// The numeric value of the quantity.
    /// </summary>
    public int Value { get; init; }

    private Quantity(int value) => Value = value;

    /// <summary>
    /// Creates a new instance of <see cref="Quantity"/> with validation.
    /// </summary>
    /// <param name="value">The quantity value to create.</param>
    /// <returns>A validated <see cref="Quantity"/> instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if value is less than 1 or greater than 100.</exception>
    public static Result<Quantity> Create(int value)
    {
        if (value < 0) return DomainErrors.Quantity.InvalidAmount;

        return new Quantity(value);
    }

    public static Quantity Empty()
    {
        return new Quantity(0);
    }

    public static Quantity Zero() => new Quantity(0);

    public static implicit operator int(Quantity quantity) => quantity.Value;
    public static explicit operator Quantity(int value) => Create(value).Value;
}
