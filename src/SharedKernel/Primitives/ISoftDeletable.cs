using SharedKernel.Primitives.Results;

namespace SharedKernel.Primitives;

public interface ISoftDeletable
{
    // Properties to track state
    bool IsDeleted { get; }
    DateTime? DeletedOnUtc { get; }

    // The two methods you requested
    Result Delete();
    Result Restore();
}