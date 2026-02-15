using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SharedKernel.Common;

public class StronglyTypedIdConverter<TId> : ValueConverter<TId, Guid> 
    where TId : struct
{
    public StronglyTypedIdConverter() 
        : base(
            id => ExtractValue(id), 
            value => CreateId(value)) 
    { }

    private static Guid ExtractValue(TId id)
    {
        var property = typeof(TId).GetProperty("Value");
        return (Guid)(property?.GetValue(id) ?? Guid.Empty);
    }

    private static TId CreateId(Guid value)
    {
        // This assumes your ID struct has a constructor: public MyId(Guid value)
        return (TId)Activator.CreateInstance(typeof(TId), value)!;
    }
}