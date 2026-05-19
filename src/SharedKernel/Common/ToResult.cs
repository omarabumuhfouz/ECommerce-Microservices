using SharedKernel.Primitives.Results;

namespace SharedKernel.Common;

public static class MappingExtensions
{
    public static Result<T> ToResult<T>(this T? value, Error error) 
        => value is not null 
            ? Result.Success(value) 
            : Result.Failure<T>(error);

    public static async Task<Result<T>> ToResult<T>(this Task<T?> task, Error error)
    {
        var value = await task;
        return value is not null ? Result.Success(value) : Result.Failure<T>(error);
    }
}