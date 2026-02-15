namespace OrderService.Domain.Enums
{
    /// <summary>
    /// Represents the type of address used in an order.
    /// </summary>
    public enum AddressType
    {
        /// <summary>
        /// Indicates a billing address.
        /// </summary>
        Billing,

        /// <summary>
        /// Indicates a shipping address.
        /// </summary>
        Shipping,

        /// <summary>
        /// Indicates another type of address (e.g., contact or pickup).
        /// </summary>
        Other
    }
}
