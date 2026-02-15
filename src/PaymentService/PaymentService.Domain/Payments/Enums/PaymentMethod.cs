using Ardalis.SmartEnum;

namespace PaymentService.Domain.Payments.Enums; // Moved to Shared

public abstract class PaymentMethod : SmartEnum<PaymentMethod>
{
    public static readonly PaymentMethod CreditCard = new CreditCardType();
    public static readonly PaymentMethod PayPal = new PayPalType();
    public static readonly PaymentMethod CashOnDelivery = new CashOnDeliveryType();

    public bool IsAutomated { get; }
    public bool RequiresExternalRedirect { get; }
    public bool SupportsAutomaticRefund { get; }

    private PaymentMethod(string name, int value, bool isAutomated, bool requiresRedirect, bool supportsAutoRefund) 
        : base(name, value)
    {
        IsAutomated = isAutomated;
        RequiresExternalRedirect = requiresRedirect;
        SupportsAutomaticRefund = supportsAutoRefund;
    }

    private sealed class CreditCardType : PaymentMethod 
    { 
        public CreditCardType() : base("CreditCard", 1, true, false, true) { } 
    }

    private sealed class PayPalType : PaymentMethod 
    { 
        public PayPalType() : base("PayPal", 2, true, true, true) { } 
    }

    private sealed class CashOnDeliveryType : PaymentMethod 
    { 
        // COD cannot be automatically refunded to a card/wallet
        public CashOnDeliveryType() : base("CashOnDelivery", 3, false, false, false) { } 
    }
}