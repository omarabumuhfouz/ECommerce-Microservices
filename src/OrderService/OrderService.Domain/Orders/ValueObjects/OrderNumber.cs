using OrderService.Domain.Constants;

namespace OrderService.Domain.Orders.ValueObjects;

public record OrderNumber
{
    public string Value { get; }

    // Private constructor guarantees validity
    public OrderNumber(){}
    private OrderNumber(string value) => Value = value;

    public static Result<OrderNumber> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) 
            return DomainErrors.OrderNumber.Empty;

        if (value.Length > DomainLimits.Order.MaxNumberLength) 
            return DomainErrors.OrderNumber.TooLong;

        return new OrderNumber(value);
    }

    // Easy string conversion
    public override string ToString() => Value;
    
    // Implicit conversion for easier usage
    public static implicit operator string(OrderNumber number) => number.Value;
}
