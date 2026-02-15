using SharedKernel.Abstractions;

namespace SharedKernel.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}