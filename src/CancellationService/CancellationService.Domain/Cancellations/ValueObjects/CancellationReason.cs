namespace CancellationService.Domain.ValueObjects
{
    public record CancellationReason
    {
        public string Value { get; }

        public CancellationReason(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Reason is required.");

            if (value.Length > 500)
                throw new ArgumentException("Reason cannot exceed 500 characters.");

            Value = value;
        }

        public static implicit operator string(CancellationReason reason) => reason.Value;
        public static explicit operator CancellationReason(string value) => new CancellationReason(value);
    }
}
