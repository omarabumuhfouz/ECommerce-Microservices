namespace OrderService.Domain.Constants;
public static class DomainLimits
{
public static class Order
{
    public const int MAX_ORDER_NUMBER_LENGTH = 30;
public const int MaxNumberLength = 50;
}

public static class OrderItem
{
    public const int MAX_QUANTITY = 100;
    public const int MIN_QUANTITY = 1;
    public const int MAX_PERCENTAGE = 100;
    public const int MIN_PERCENTAGE = 0;
}
    
}