namespace CancellationService.Domain.ValueObjects
{
    public record CancellationRemarks
    {
        public string Value { get; }

        public CancellationRemarks(string value)
        {
            if (value.Length > 500)
                throw new ArgumentException("Remarks cannot exceed 500 characters.");

            Value = value;
        }

        public static implicit operator string(CancellationRemarks remarks) => remarks.Value;
        public static explicit operator CancellationRemarks(string value) => new CancellationRemarks(value);
    }
}
