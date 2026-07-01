using InventoryService.Domain.Errors;
using SharedKernel.Primitives.Results;

namespace InventoryService.Domain.ValueObjects;

public sealed record Quantity
{
    public int Value { get; }

    private Quantity(int value)
    {
        Value = value;
    }

    public static Result<Quantity> Create(int value)
    {
        if (value < 0) 
            return DomainErrors.Inventory.NegativeQuantity;

        return new Quantity(value);
    }

    public static Quantity Zero() => new(0);

    // Helper methods to make the Aggregate logic clean
    public Result<Quantity> Add(int amount) => Create(Value + amount);
    public Result<Quantity> Subtract(int amount) => Create(Value - amount);
}